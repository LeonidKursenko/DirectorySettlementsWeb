using DirectorySettlementsDAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DirectorySettlementsDALTests.Helpers
{
    internal class TestData
    {
        public static IEnumerable<Settlement> InitSettlementsData()
        {
            var settlement1 = new Settlement { Te = "0100000000", Nu = "АВТОНОМНА РЕСПУБЛІКА КРИМ/М.СІМФЕРОПОЛЬ" };
            var settlement2 = new Settlement { Te = "0110000000", ParentId = "0100000000", Parent = settlement1 };
            //settlement1.Children.Add(settlement2);
            return new List<Settlement>()
            {
                settlement1,
                settlement2,
                new Settlement { Te = "0110100000", ParentId = "0110000000"},
                new Settlement { Te = "0110130000", ParentId = "0110100000"},
                new Settlement { Te = "0110136300", ParentId = "0110130000"},
                new Settlement { Te = "0110136600", ParentId = "0110130000"},
                new Settlement { Te = "0110136900", ParentId = "0110130000"},
                new Settlement { Te = "0110165000", ParentId = "0110100000"},
                new Settlement { Te = "0110165300", ParentId = "0110165000"},
                new Settlement { Te = "0110165600", ParentId = "0110165000"},
                new Settlement { Te = "0110165601", ParentId = "0110165600"},

                new Settlement { Te = "0110300000"},
                new Settlement { Te = "0110345000"},
                new Settlement { Te = "0110345400"},

                new Settlement { Te = "0111200000"},
                new Settlement { Te = "0111600000"},
                new Settlement { Te = "0111645000"},
                new Settlement { Te = "0111645200"},
                new Settlement { Te = "0111645400"},
                new Settlement { Te = "0111645700"},
                new Settlement { Te = "0111645701"},
                new Settlement { Te = "0111646000"},

                new Settlement { Te = "0120000000"},
                new Settlement { Te = "0200000000"},
                new Settlement { Te = "8000000000"}
            };
        }

        public static IEnumerable<InitialTable> InitialTableData()
        {
            return new List<InitialTable>()
            {
                new InitialTable { Te = "0100000000", Nu = "АВТОНОМНА РЕСПУБЛІКА КРИМ/М.СІМФЕРОПОЛЬ"},
                new InitialTable { Te = "0110000000", Nu = "МІСТА АВТОНОМНОЇ РЕСПУБЛІКИ КРИМ"},
                new InitialTable { Te = "0110100000", Nu = "СІМФЕРОПОЛЬ"},
                new InitialTable { Te = "0110130000", Nu = "РАЙОНИ М.СІМФЕРОПОЛЯ"},
                new InitialTable { Te = "0110136300", Nu = "ЗАЛІЗНИЧНИЙ", Np = "Р"},
                new InitialTable { Te = "0110136600", Nu = "КИЇВСЬКИЙ", Np = "Р"},
                new InitialTable { Te = "0110136900", Nu = "ЦЕНТРАЛЬНИЙ", Np = "Р"},
                new InitialTable { Te = "0110165000", Nu = "СЕЛИЩА МІСЬКОГО ТИПУ, ПІДПОРЯДКОВАНІ ЗАЛІЗНИЧНІЙ РАЙРАДІ М.СІМФЕРОПОЛЯ"},
                new InitialTable { Te = "0110165300", Nu = "АЕРОФЛОТСЬКИЙ", Np = "Т"},
                new InitialTable { Te = "0110165600", Nu = "ГРЕСІВСЬКИЙ", Np = "Т"},
                new InitialTable { Te = "0110165601", Nu = "БІТУМНЕ", Np = "Щ"}
            };
        }
    }
}
