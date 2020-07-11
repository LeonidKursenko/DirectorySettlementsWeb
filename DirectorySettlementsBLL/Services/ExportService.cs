using DirectorySettlementsBLL.DTO;
using DirectorySettlementsBLL.Interfaces;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirectorySettlementsBLL.Services
{
    public class ExportService : IExportService
    {
        public byte[] Export(IEnumerable<SettlementDTO> settlements)
        {
            string s = BuildHtml(settlements as ICollection<SettlementDTO>);

            var Renderer = new IronPdf.HtmlToPdf();
            var PDF = Renderer.RenderHtmlAsPdf(s);
            return PDF.BinaryData;

        }

        private StringBuilder _stringBuilder = new StringBuilder();
        private string BuildHtml(ICollection<SettlementDTO> settlements)
        {
            _stringBuilder.Append("<h1>КОАТУУ</h1>");
            _stringBuilder.Append("<ul>");
            BuildTree(settlements);
            _stringBuilder.Append("</ul>");
            return _stringBuilder.ToString();
        }

        private void BuildTree(ICollection<SettlementDTO> settlements)
        {
            foreach (var settlement in settlements)
            {

                _stringBuilder.Append($"<li>{settlement.Nu}[{settlement.Te}] {settlement.Np ?? ""}</li>");
                if (settlement.Children.Any() == true)
                {
                    _stringBuilder.Append("<ul>");
                    BuildTree(settlement.Children);
                    _stringBuilder.Append("</ul>");
                }
            }
        }
    }
}
