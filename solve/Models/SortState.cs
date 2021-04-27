using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace solve.Models
{
    public enum SortState
    {
        StatusAsc,    // по имени по возрастанию
        StatusDesc,   // по имени по убыванию
        AgeAsc, // по возрасту по возрастанию
        AgeDesc,    // по возрасту по убыванию
        CompanyAsc, // по компании по возрастанию
        CompanyDesc // по компании по убыванию
    }
}
