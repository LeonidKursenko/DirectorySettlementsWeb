using DirectorySettlementsDAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorySettlementsBLLTests.Helpers
{
    internal class TestData
    {
        public static IEnumerable<Settlement> EmitData()
        {
            return new List<Settlement>()
            {
                new Settlement { Te = "0100000000", Nu = "АВТОНОМНА РЕСПУБЛІКА КРИМ/М.СІМФЕРОПОЛЬ"},
                new Settlement { Te = "0110000000", Nu = "МІСТА АВТОНОМНОЇ РЕСПУБЛІКИ КРИМ"},
                new Settlement { Te = "0110100000", Nu = "СІМФЕРОПОЛЬ"},
                new Settlement { Te = "0110130000", Nu = "РАЙОНИ М.СІМФЕРОПОЛЯ"},
                new Settlement { Te = "0110136300", Nu = "ЗАЛІЗНИЧНИЙ", Np = "Р"},
                new Settlement { Te = "0110136600", Nu = "КИЇВСЬКИЙ", Np = "Р"},
                new Settlement { Te = "0110136900", Nu = "ЦЕНТРАЛЬНИЙ", Np = "Р"},
                new Settlement { Te = "0110165000", Nu = "СЕЛИЩА МІСЬКОГО ТИПУ, ПІДПОРЯДКОВАНІ ЗАЛІЗНИЧНІЙ РАЙРАДІ М.СІМФЕРОПОЛЯ"},
                new Settlement { Te = "0110165300", Nu = "АЕРОФЛОТСЬКИЙ", Np = "Т"},
                new Settlement { Te = "0110165600", Nu = "ГРЕСІВСЬКИЙ", Np = "Т"},
                new Settlement { Te = "0110165601", Nu = "БІТУМНЕ", Np = "Щ"}
            };
        }
    }
}
