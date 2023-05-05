using System;
using System.Data;
using System.Data.Odbc;
using System.Windows.Forms;

namespace ReservaVuelos
{
    public partial class Reservas : Form
    {
        OdbcConnection conn;
        public Reservas()
        {
            // Conexión a la base de datos con ODBC, el DSN es el nombre de la conexión
            conn = new OdbcConnection("Dsn=Reservas");

            // Inicializa los componentes de la ventana
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Valores de los datetimepicker al cargar el formulario
            dateTimePicker1.Value = new DateTime(2021, 10, 01);
            dateTimePicker2.Value = new DateTime(2021, 10, 31);

            // Carga los datos de vuelos sin filtrar en el listado
            listarVuelos();

        }

        private void combobox1(object sender, EventArgs e)
        {
            // Cargar datos de aeropuertos para el combobox1 al hacer click para desplegar
            String sqlAeropuertos = "SELECT Nombre FROM Aerpuertos ORDER BY Nombre ASC";
            DataSet datos = new DataSet();
            OdbcDataAdapter adapter = new OdbcDataAdapter(sqlAeropuertos, conn);
            adapter.Fill(datos, "DataTable1");
            DataTable aeropuertos = datos.Tables["DataTable1"];

            comboBox1.Items.Clear();
            foreach (DataRow row in aeropuertos.Rows)
            {
                comboBox1.Items.Add(row["Nombre"].ToString());
            }
        }

        private void combobox2(object sender, EventArgs e)
        {
            // Cargar datos de aeropuertos para el combobox1 al hacer click para desplegar
            String sqlAeropuertos = "SELECT Nombre FROM Aerpuertos ORDER BY Nombre ASC";
            DataSet datos = new DataSet();
            OdbcDataAdapter adapter = new OdbcDataAdapter(sqlAeropuertos, conn);
            adapter.Fill(datos, "DataTable1");
            DataTable aeropuertos = datos.Tables["DataTable1"];

            comboBox2.Items.Clear();
            foreach (DataRow row in aeropuertos.Rows)
            {
                comboBox2.Items.Add(row["Nombre"].ToString());
            }
        }

        private void listarVuelos()
        {
            String sqlReservas = "SELECT Vuelos.Numero, CAST(Vuelos.Fecha AS DATE) AS Fecha, Vuelos.Hora, Aerpuertos.Nombre AS Origen, Aerpuertos_1.Nombre AS Destino, Aerolineas.Nombre AS Aerolinea"
                + " " + "FROM Aerolineas, Vuelos, Aerpuertos, Aerpuertos Aerpuertos_1" + " "
                + "WHERE Aerolineas.Id = Vuelos.Aerolinea AND Vuelos.Origen = Aerpuertos.Id AND Vuelos.Destino = Aerpuertos_1.Id";


            //Anadir dateTimePicker1 a consulta sql
            if (dateTimePicker1.Value != null)
            {
                sqlReservas += " " + "AND Vuelos.Fecha >= '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "'";
            }

            //Anadir dateTimePicker2 a consulta sql con fecha entre datetimepicker 1 y 2
            if (dateTimePicker2.Value != null)
            {
                sqlReservas += " " + "AND Vuelos.Fecha <= '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "'";
            }

            //Anadir comboBox1 a consulta sql
            if (comboBox1.Text != "")
            {
                sqlReservas += " " + "AND Aerpuertos.Nombre = '" + comboBox1.Text + "'";

            }

            //Anadir comboBox2 a consulta sql
            if (comboBox2.SelectedIndex != -1)
            {
                sqlReservas += " " + "AND Aerpuertos_1.Nombre = '" + comboBox2.Text + "'";
            }

            sqlReservas += " " + "ORDER BY Fecha asc, Hora asc;";

            DataSet datos = new DataSet();
            OdbcDataAdapter adapter = new OdbcDataAdapter(sqlReservas, conn);
            adapter.Fill(datos, "DataTable1");
            DataTable vuelos = datos.Tables["DataTable1"];

            listView1.Items.Clear();
            foreach (DataRow row in vuelos.Rows)
            {
                ListViewItem item = new ListViewItem(row["Numero"].ToString());
                item.SubItems.Add(DateTime.Parse(row["Fecha"].ToString()).ToString("dd/MM/yyyy"));
                item.SubItems.Add(row["Hora"].ToString());
                item.SubItems.Add(row["Origen"].ToString());
                item.SubItems.Add(row["Destino"].ToString());
                item.SubItems.Add(row["Aerolinea"].ToString());
                listView1.Items.Add(item);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            listarVuelos();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Vacio los campos de los combobox y los datetimepicker
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            dateTimePicker1.Value = new DateTime(2021, 10, 01);
            dateTimePicker2.Value = new DateTime(2021, 10, 31);
            listarVuelos();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Nueva ventana con los datos del vuelo y elegir asiento
            Informe form2 = new Informe(listView1.SelectedItems[0].Text);
            form2.ShowDialog();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Informe informe = new Informe(dateTimePicker1.Value.ToString("yyyy-MM-dd"), dateTimePicker2.Value.ToString("yyyy-MM-dd"), comboBox1.Text, comboBox2.Text);
            informe.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            try
            {
                // Valor seleccionado de la lista
                string rowSelected = listView1.SelectedItems[0].Text;

                // Comprobar que se rellena al menos un checkbox y se pone una cantidad en el campo numerico
                if (checkBox1.Checked == false && checkBox2.Checked == false)
                {
                    MessageBox.Show("Debe seleccionar al menos un tipo de billete");
                }
                else if (numericUpDown1.Value == 0 && numericUpDown2.Value == 0)
                {
                    MessageBox.Show("Debe seleccionar al menos un billete");
                }
                else if (comboBox3.Items.Count == 0)
                {
                    MessageBox.Show("Debe seleccionar asiento");
                }
                else
                {
                    // Mensaje con datos de billetes
                    String mensaje = "Vuelo: " + rowSelected.Substring(0, rowSelected.Length - 1) + "\n";
                    mensaje += "Adultos: " + checkBox1.Checked + "\n";
                    mensaje += "Billetes: " + numericUpDown1.Value + "\n";
                    if (checkBox2.Checked == true)
                    {
                        mensaje += "Menores: " + checkBox2.Checked + "\n";
                        mensaje += "Billetes: " + numericUpDown2.Value + "\n";
                    }
                    mensaje += "Asiento: " + comboBox3.Text + "\n";

                    // Pop up con mensaje
                    MessageBox.Show(mensaje, "Reserva");

                }

            }
            catch
            {
                MessageBox.Show("Debe seleccionar un vuelo.");
            }


        }

        private void verLaAyudaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "App Reservas de Vuelos.chm");
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Acerca de en un message box
            MessageBox.Show("Aplicación de gestión de reservas de vuelos\n" +
                "Desarrollada por:\n" +
                "Alberto Pérez del Río\n");

        }

        private void Reservas_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Help.ShowHelp(this, "App Reservas de Vuelos.chm");
        }
    }
}
