using System;
using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;
using EllipticCurve;
using Grpc.Net.Client;
using GrpcService;
using static GrpcService.BChainService;

namespace Billiard.paapCWallet
{

    public class BilliardService
    {
        public BChainServiceClient service;
        public Account account;

        public BilliardService(string sec)
        {

            var serverAddress = "http://37.152.187.156:5002";
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            GrpcChannel channel = GrpcChannel.ForAddress(serverAddress);
            service = new BChainServiceClient(channel);
            account = new Account(sec);
        }
        public string GetAddress()
        {
            var address = account.GetAddress();
            return address;
        }
        public double GetBalance()
        {
            var address = account.GetAddress();
            if (string.IsNullOrEmpty(address))
            {

                return 0;
            }


            try
            {
                var response = service.GetBalance(new AccountRequest
                {
                    Address = address
                });
                return response.Balance;

            }
            catch  
            {
                return 0;
            }

        }


        public string SendCoin(string recipient, string strAmount, string strFee)
        {

            string sender = account.GetAddress();

            double amount;


            amount = double.Parse(strAmount);


            float fee;

            fee = float.Parse(strFee);



            var response = service.GetBalance(new AccountRequest
            {
                Address = sender
            });

            var senderBalance = response.Balance;


            if ((amount + fee) > senderBalance)
            {

                return "don't have enough balance!";
            }

            var trxin = new TrxInput
            {
                SenderAddress = account.GetAddress(),
                TimeStamp = Utils.GetTime()
            };

            var trxOut = new TrxOutput
            {
                RecipientAddress = recipient,
                Amount = amount,
                Fee = fee,
            };

            var trxHash = Utils.GetTransactionHash(trxin, trxOut);
            var signature = account.CreateSignature(trxHash);


            trxin.Signature = signature;

            var sendRequest = new SendRequest
            {
                TrxId = trxHash,
                PublicKey = account.GetPubKeyHex(),
                TrxInput = trxin,
                TrxOutput = trxOut
            };

            try
            {
                var responseSend = service.SendCoin(sendRequest);

                if (responseSend.Result.ToLower() == "success")
                {
                    DateTime utcDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(Convert.ToDouble(trxin.TimeStamp));

                    return "Need around 30 second to be processed!";
                }
                else
                {
                     return string.Format("Error: {0}", responseSend.Result);
                }

            }
            catch (Exception e)
            {
                 return string.Format("Error: {0}", e.Message);
            }

        }



    }
}