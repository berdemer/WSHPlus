using ISG.Entities.ComplexType;
using ISG.Entities.Concrete;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
	public interface IExcelAzureService
	{
        Task<List<ExpandoObject>> AzurDeposundakiExceldenVeriAl(string accountName, string accountKey, string folderName, string fileName, int sheetIndex);


    }
}
