using TheLostSpirit.Application.Port.InstanceContext.InstanceContext;
using TheLostSpirit.Application.Port.InstanceContext.InstanceFactory;
using TheLostSpirit.Domain.Formula;
using TheLostSpirit.Identity.ConfigID;
using TheLostSpirit.Infrastructure.Database;
using UnityEngine;

namespace TheLostSpirit.Context.Formula
{
    public class ManifestationInstanceFactory : IManifestationInstanceFactory
    {
        readonly ManifestationDatabase _manifestationDatabase;

        public ManifestationInstanceFactory(ManifestationDatabase manifestationDatabase) {
            _manifestationDatabase = manifestationDatabase;
        }


        public IManifestationInstanceContext Create(ManifestationConfigID configID, FormulaPayload payload) {
            var config   = _manifestationDatabase.GetByID(configID);
            var instance = Object.Instantiate(config).GetComponent<ManifestationInstanceContext>();
            instance.Construct(payload);

            return instance;
        }
    }
}