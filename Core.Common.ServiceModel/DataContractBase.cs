using System.Runtime.Serialization;

namespace Core.Common.ServiceModel
{
    public class DataContractBase : IExtensibleDataObject
    {
        public ExtensionDataObject ExtensionData { get; set; }
    }
}
