using Sowalabs.Bison.Common.BisonApi;
using Sowalabs.Bison.Common.Trading;
using Sowalabs.Bison.Data.Types;
using Sowalabs.Bison.DataAccessLayer;
using Sowalabs.Bison.LiquidityEngine.Dependencies;
using Sowalabs.Bison.LiquidityEngine.TaskGenerators;
using System;

namespace Sowalabs.Bison.LiquidityEngine
{
    public class LiquidityEngine : ILiquidityProviderService, IDisposable
    {
        private readonly IDependencyFactory _dependencyFactory;
        private readonly UserMoneyTransfer _dalTransfer = new UserMoneyTransfer();

        internal UserMoneyTransferGenerator UserMoneyTransferGenerator { get; }
        internal IQueue Queue { get; }
        public bool IsDisposed { get; private set; }

        public LiquidityEngine() : this(new LiquidityEngineDependencyFactory())
        {
        }

        public LiquidityEngine(IDependencyFactory dependencyFactory, IQueue queue = null)
        {
            _dependencyFactory = dependencyFactory;
            UserMoneyTransferGenerator = new UserMoneyTransferGenerator(dependencyFactory, this);
            Queue = queue ?? new Queue(dependencyFactory);

            //TODO Remove this test setting.
            UserMoneyTransferGenerator.Delay = 5;
        }

        public void Initialize()
        {
            UserMoneyTransferGenerator.Initialize();
        }

 
        public void RegisterAcceptedOffer(Offer offer)
        {
            var transfer = new Data.UserMoneyTransfer
            {
                Currency = offer.Currency.ToString(),
                Amount = Math.Round(offer.BuySell == BuySell.Buy ? offer.Amount * offer.Price : -offer.Amount * offer.Price, 2),
                Reference = offer.Reference,
                OpenTimestamp = offer.OpenTimeStamp,
                Status = UserMoneyTransferStatus.New,
                UserId = offer.UserId,
                UserAccount = offer.UserAccount
            };

            try
            {
                _dalTransfer.Insert(transfer);
            }
            catch (DuplicateException)
            {
                // Duplication should mean service call is being repetead as a recovery from error on callers side.
                // So a duplication is OK (implies a caller's crash during or after previous successful call).
                return;
            }

            UserMoneyTransferGenerator.RegisterNewTransfer(transfer);



            //if (offer.BuySell == BuySell.Buy)
            //{
            //    var solaris = EuwaxData.Solaris;
            //    var data = new MoneyTransferData
            //    {
            //        FromSwift = solaris.SwiftCode,
            //        FromIban = "CustomerIBAN",
            //        ToSwift = solaris.SwiftCode,
            //        ToIban = solaris.GetAccount(offer.Currency).Iban,
            //        Amount = Math.Round(offer.Amount * offer.Price, 2)
            //    };
            //    Queue.Enqueue(new TransferFromCustomerTask(_dependencyFactory, this, data));
            //}
            //else
            //{
            //    var solaris = EuwaxData.Solaris;
            //    var data = new MoneyTransferData
            //    {
            //        FromSwift = solaris.SwiftCode,
            //        FromIban = solaris.GetAccount(offer.Currency).Iban,
            //        ToSwift = solaris.SwiftCode,
            //        ToIban = "CustomerIBAN",
            //        Amount = Math.Round(offer.Amount * offer.Price, 2)
            //    };
            //    Queue.Enqueue(new TransferToCustomerTask(_dependencyFactory, this, data));
            //}
        }

        #region Dispose

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            IsDisposed = true;
            UserMoneyTransferGenerator?.Dispose();
            Queue.Dispose();
        }

        #endregion

    }
}
