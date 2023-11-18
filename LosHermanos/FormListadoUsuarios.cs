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
    public partial class FormListadoUsuarios : Form
    {
        private Conexion conexion;

        public FormListadoUsuarios()
        {
            InitializeComponent();
            conexion = new Conexion();
        }

        private void buscar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = txt_buscar.Text;

                // Abre la conexión a la base de datos
                conexion.AbrirConexion();

                // Consulta para buscar Usuarios por nombre o NIT
                string query = "SELECT Codigo, NombreUsuario, Nombres, Apellidos,CorreoElectronico, Contraseña FROM Usuarios WHERE Codigo LIKE '%" + nombre + "%' OR Nombres LIKE '%" + nombre + "%' OR NombreUsuario LIKE '%" + nombre + "%'";

                // Crea un adaptador de datos y un DataSet
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, conexion.con);
                DataSet dataSet = new DataSet();

                // Llena el DataSet con los datos obtenidos de la consulta
                adapter.Fill(dataSet, "Usuarios");

                // Asigna los datos al DataGridView
                dataGridView1.DataSource = dataSet.Tables["Usuarios"];

                // Cierra la conexión a la base de datos
                conexion.CerrarConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar los Usuarios: " + ex.Message);
            }

        }

        private void FormListadoUsuarios_Load(object sender, EventArgs e)
        {
            try
            {
                // Abre la conexión a la base de datos
                conexion.AbrirConexion();

                // Consulta para obtener los Usuarios
                string query = "SELECT Codigo, NombreUsuario, Nombres, Apellidos, CorreoElectronico, Contraseña FROM Usuarios";

                // Crea un adaptador de datos y un DataSet
                OleDbDataAdapter adapter = new OleDbDataAdapter(query, conexion.con);
                DataSet dataSet = new DataSet();

                // Llena el DataSet con los datos obtenidos de la consulta
                adapter.Fill(dataSet, "Usuarios");

                // Asigna los datos al DataGridView
                dataGridView1.DataSource = dataSet.Tables["Usuarios"];

                // Cierra la conexión a la base de datos
                conexion.CerrarConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al listar los Usuarios: " + ex.Message);
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
