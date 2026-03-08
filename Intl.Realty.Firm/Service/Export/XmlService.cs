using Intl.Realty.Firm.Service.IService.IExport;
using System.Xml.Serialization;

namespace Intl.Realty.Firm.Service.Export
{
    public class XmlService : IXmlService
    {
        public byte[] Write<T>(IList<T> registers)
        {
            var serializer = new XmlSerializer(typeof(List<T>));

            using (MemoryStream memoryStream = new MemoryStream())
            {
                serializer.Serialize(memoryStream, registers);

                return memoryStream.ToArray();
            }
        }
    }
}
