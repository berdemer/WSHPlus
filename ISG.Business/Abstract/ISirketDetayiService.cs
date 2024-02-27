using ISG.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISG.Business.Abstract
{
    public interface ISirketDetayiService
    {
        Task<SirketDetayi> GetAsync(int id);

        Task<SirketDetayi> AddAsync(SirketDetayi sirketDetayi);

        Task<SirketDetayi> UpdateAsync(SirketDetayi sirketDetayi, int key);

        Task<int> DeleteAsync(int key);

    }
}
