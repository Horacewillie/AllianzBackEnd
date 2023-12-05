using AllianzBackEnd.Domain;
using AllianzBackEnd.Domain.Base.Entities.PurchaseHistories;
using AllianzBackEnd.Domain.Base.Entities.Users;
using AllianzBackEnd.Domain.Config;
using AllianzBackEnd.Domain.Enums;
using AllianzBackEnd.Domain.Helpers;
using AllianzBackEnd.Domain.Models;
using AllianzBackEnd.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AllianzBackEnd.Core.Managers
{

    public class PurchaseHistoryManager
    {
        private readonly IPurchaseHistoryRepository _purchaseHistoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly ApiClient _apiClient;
        private readonly FlutterWaveConfig _flutterwaveConfig;
        public PurchaseHistoryManager(IPurchaseHistoryRepository purchaseHistoryRepository, ApiClient apiClient, FlutterWaveConfig flutterwaveConfig, IUserRepository userRepository)
        {
            _purchaseHistoryRepository = purchaseHistoryRepository;
            _apiClient = apiClient;
            _flutterwaveConfig = flutterwaveConfig;
            _userRepository = userRepository;
        }
        public async Task<bool> MakePayment(PurchaseRequest request)
        {
            var user = await _userRepository.Find(request.UserId)
                ?? throw new Exception("Could not fetch user");

            var purchaseHistory = new PurchaseHistory(request)
            {
                TransactionReference = TransactionReferenceHelper.GenerateTransactionReference()
            };

            //_purchaseHistoryRepository.Add(purchaseHistory);
            var createdPurchaseHistory = _userRepository.AddPurchaseHistory(purchaseHistory);

            user.AddPurchaseHistory(createdPurchaseHistory);

            //await _purchaseHistoryRepository.SaveChanges();

            var transferData = new
            {
                account_bank = "044",
                account_number = "0690000040",
                amount = request.Amount,
                narration = "Allianz Car Insurance Payment",
                currency = "NGN",
                callback_url = "",
                reference = createdPurchaseHistory.TransactionReference
            };

            var response = await _apiClient.Post<FlutterwaveTransferResponse>(
                transferData,
                _flutterwaveConfig?.BankTransferUrl!,
                _flutterwaveConfig?.ProviderApiKey!
            );

            if (response.Data is null)
            {
                throw new Exception(response.Message!);
            }

            createdPurchaseHistory.UpdateTransactionStatus(
                response.Status == ResultStatus.Complete
                    ? TransactionStatus.Complete
                    : TransactionStatus.Failed
            );

            await _userRepository.SaveChanges();

            // Return true if the payment is complete; otherwise, return false
            return response.Status == ResultStatus.Complete;
        }

        public async Task<PurchaseHistoryResponse> GetPurchaseHistory(GetPurchaseHistoryRequest request)
        {

            var (purchaseHistories, pagingData) = await _purchaseHistoryRepository.GetPuchaseHistories(request);
            return new PurchaseHistoryResponse
            {
                PagingData = pagingData,
                PurchaseHistories = purchaseHistories
            };
        }
    }
}
