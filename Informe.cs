using Microsoft.Reporting.WinForms;
using System;
using System.Windows.Forms;

namespace ReservaVuelos
{
    public partial class Informe : Form
    {
        String dateFrom;
        String dateTo;
        String origen;
        String destino;

        public Informe(String dateFrom, String dateTo, String origen, String destino)
        {
            InitializeComponent();
            this.dateFrom = dateFrom;
            this.dateTo = dateTo;
            this.origen = origen;
            this.destino = destino;
        }

        public Informe()
        {
            InitializeComponent();
        }

        public Informe(string text)
        {
            Text = text;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'dataSet1.DataTable1' Puede moverla o quitarla según sea necesario.
            this.dataTable1TableAdapter.Fill(this.dataSet1.DataTable1);

            ReportParameter p1 = new ReportParameter("ParamDateFrom", dateFrom);
            ReportParameter p2 = new ReportParameter("ParamDateTo", dateTo);
            ReportParameter p3 = new ReportParameter("ParamFrom", origen);
            ReportParameter p4 = new ReportParameter("ParamTo", destino);
            this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4 });

            this.reportViewer1.RefreshReport();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
