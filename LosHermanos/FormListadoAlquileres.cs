using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LosHermanos
{
    public partial class FormListadoAlquileres : Form
    {
        private Conexion conexion;

        public FormListadoAlquileres()
        {
            InitializeComponent();
            conexion = new Conexion();

        }

        private void FormListadoAlquileres_Load(object sender, EventArgs e)
        {
            try
            {
                // Abre la conexión a la base de datos
                conexion.AbrirConexion();

                // Consulta para obtener los Alquileres
                string query = "SELECT CodigoAlquiler,FechaInicio, FechaDevolucion, CodigoAutomovil, CodigoCliente, NITCliente, NombreCliente FROM Alquileres";

                // Crea un adaptador de datos y un DataSet
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, conexion.con);
                DataSet dataSet = new DataSet();

                // Llena el DataSet con los datos obtenidos de la consulta
                adapter.Fill(dataSet, "Alquileres");

                // Asigna los datos al DataGridView
                dataGridView1.DataSource = dataSet.Tables["Alquileres"];

                // Cierra la conexión a la base de datos
                conexion.CerrarConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al listar los Alquileres: " + ex.Message);
            }

        }

        private void buscar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = txt_buscar.Text;

                // Abre la conexión a la base de datos
                conexion.AbrirConexion();

                // Consulta para buscar Alquileres por nombre o NIT
                string query = "SELECT CodigoAlquiler, FechaInicio, FechaDevolucion, CodigoAutomovil, CodigoCliente, NITCliente, NombreCliente FROM Alquileres WHERE CodigoAlquiler LIKE '%" + nombre + "%' OR NITCliente LIKE '%" + nombre + "%' OR NombreCliente LIKE '%" + nombre + "%'";

                // Crea un adaptador de datos y un DataSet
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, conexion.con);
                DataSet dataSet = new DataSet();

                // Llena el DataSet con los datos obtenidos de la consulta
                adapter.Fill(dataSet, "Alquileres");

                // Asigna los datos al DataGridView
                dataGridView1.DataSource = dataSet.Tables["Alquileres"];

                // Cierra la conexión a la base de datos
                conexion.CerrarConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar los Alquileres: " + ex.Message);
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
