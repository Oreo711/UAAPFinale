using System;
using _Project.Develop.Runtime.Configs.Gameplay;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.Deploy;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using UnityEngine;


namespace _Project.Develop.Runtime.Gameplay.Features.Deploy
{
	public class DeployablePurchaseService
	{
		private readonly WalletService        _wallet;
		private readonly DeployableCostConfig _costConfig;

		public DeployablePurchaseService (WalletService wallet, ICoroutinesPerformer coroutinesPerformer, DeployableCostConfig costConfig)
		{
			_wallet     = wallet;
			_costConfig = costConfig;
		}

		public void PurchaseAndDeploy (Deployables deployable, Vector3 deployPoint, Entity deployer)
		{
			switch (deployable)
			{
				case Deployables.Mine:
					if (!_wallet.Enough(CurrencyTypes.Gold, _costConfig.MineCost))
					{
						return;
					}

					deployer.MineDeployRequest.Invoke(deployPoint);

					_wallet.Spend(CurrencyTypes.Gold, _costConfig.MineCost);
					break;

				case Deployables.Sentry:
					if (!_wallet.Enough(CurrencyTypes.Gold, _costConfig.SentryCost))
					{
						return;
					}

					deployer.SentryDeployRequest.Invoke(deployPoint);

					_wallet.Spend(CurrencyTypes.Gold, _costConfig.SentryCost);
					break;

				case Deployables.Puddle:
					if (!_wallet.Enough(CurrencyTypes.Gold, _costConfig.PuddleCost))
					{
						return;
					}

					deployer.PuddleDeployRequest.Invoke(deployPoint);

					_wallet.Spend(CurrencyTypes.Gold, _costConfig.PuddleCost);
					break;
				default:
					throw new ArgumentException("This type of deployable is not supported.");
			}
		}
	}
}
