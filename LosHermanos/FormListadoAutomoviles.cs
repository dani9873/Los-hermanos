using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LosHermanos
{
    public partial class FormListadoAutomoviles : Form
    {
        private Conexion conexion;
        public FormListadoAutomoviles()
        {
            InitializeComponent();
            conexion = new Conexion();
        }

        private void FormListadoAutomoviles_Load(object sender, EventArgs e)
        {
            try
            {
                // Abre la conexión a la base de datos
                conexion.AbrirConexion();

                // Consulta para obtener los clientes
                string query = "SELECT Codigo, Marca, Modelo, AnioFabricacion, PrecioPorDia, Disponibilidad FROM Automoviles";

                // Crea un adaptador de datos y un DataSet
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, conexion.con);
                DataSet dataSet = new DataSet();

                // Llena el DataSet con los datos obtenidos de la consulta
                adapter.Fill(dataSet, "Automoviles");

                // Asigna los datos al DataGridView
                dataGridView1.DataSource = dataSet.Tables["Automoviles"];
                dataGridView1.Columns["PrecioPorDia"].DefaultCellStyle.Format = "C2";
                dataGridView1.Columns["PrecioPorDia"].DefaultCellStyle.FormatProvider = CultureInfo.GetCultureInfo("es-GT");

                // Cierra la conexión a la base de datos
                conexion.CerrarConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al listar los Automóviles: " + ex.Message);
            }
        }

        private void buscar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = txt_buscar.Text;

                // Abre la conexión a la base de datos
                conexion.AbrirConexion();

                // Consulta para obtener los automóviles que coinciden con la búsqueda
                string query = "SELECT Codigo, Marca, Modelo, AnioFabricacion, PrecioPorDia, Disponibilidad FROM Automoviles " +
                               "WHERE Codigo LIKE '%" + nombre + "%' OR Marca LIKE '%" + nombre + "%' OR Modelo LIKE '%" + nombre + "%'";

                // Crea un adaptador de datos y un DataSet
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, conexion.con);
                DataSet dataSet = new DataSet();

                // Llena el DataSet con los datos obtenidos de la consulta
                adapter.Fill(dataSet, "Automoviles");

                // Asigna los datos al DataGridView
                dataGridView1.DataSource = dataSet.Tables["Automoviles"];

                // Cierra la conexión a la base de datos
                conexion.CerrarConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al realizar la búsqueda: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormMenuPrincipal formMenuPrincipal = new FormMenuPrincipal();
            formMenuPrincipal.ShowDialog();
            this.Close();
        }
    }
}
