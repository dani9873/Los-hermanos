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
    public partial class FormMantenimientoUsuarios : Form
    {
        private Conexion conexion;


        public FormMantenimientoUsuarios()
        {
            InitializeComponent();
            conexion = new Conexion();
        }

        private void CargarUsuarios()
        {
            DataTable dt = conexion.ObtenerRegistros("Usuarios");
            dataGridView1.DataSource = dt;
        }


        private void LimpiarCampos()
        {
            txtCodigo.Text = string.Empty;
            txtMarca.Text = string.Empty;
            txtModelo.Text = string.Empty;
            txtAnioFabricacion.Text = string.Empty;
            txtPrecioPorDia.Text = string.Empty;
            txtNit.Text = string.Empty;
        }

        private void FormMantenimientoUsuarios_Load(object sender, EventArgs e)
        {
            CargarUsuarios();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                string codigo = txtCodigo.Text;
            // Verificar la condición de la identificación
            if (!codigo.StartsWith("U") || !codigo.Substring(1).All(char.IsDigit))
            {
                MessageBox.Show("El código debe comenzar con 'U' seguido de números.");
                return;
            }
            string NombreUsuario = txtMarca.Text;
            string Nombres = txtModelo.Text;
            string Apellidos = txtAnioFabricacion.Text;
            string CorreoElectronico = txtPrecioPorDia.Text;
            string Contraseña = txtNit.Text;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Codigo"].Value != null && row.Cells["Codigo"].Value.ToString() == codigo)
                {
                    MessageBox.Show("El código ya existe. Por favor, ingrese uno nuevo.");
                    return;
                }
            }

            Dictionary<string, object> datos = new Dictionary<string, object>();
            datos.Add("Codigo", codigo);
            datos.Add("NombreUsuario", NombreUsuario);
            datos.Add("Nombres", Nombres);
            datos.Add("Apellidos", Apellidos);
            datos.Add("CorreoElectronico", CorreoElectronico);
            datos.Add("Contraseña", Contraseña);

            conexion.InsertarRegistro("Usuarios", datos);

            LimpiarCampos();
            CargarUsuarios();
                MessageBox.Show("El usuario se ha insertado correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar el usuario: " + ex.Message);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string codigo = txtCodigo.Text;
                string NombreUsuario = txtMarca.Text;
                string Nombres = txtModelo.Text;
                string Apellidos = txtAnioFabricacion.Text;
                string CorreoElectronico = txtPrecioPorDia.Text;
                string Contraseña = txtNit.Text;

                Dictionary<string, object> datos = new Dictionary<string, object>();
                datos.Add("NombreUsuario", NombreUsuario);
                datos.Add("Nombres", Nombres);
                datos.Add("Apellidos", Apellidos);
                datos.Add("CorreoElectronico", CorreoElectronico);
                datos.Add("Contraseña", Contraseña);

                string condicion = "Codigo = '" + codigo + "'";

                conexion.ModificarRegistro("Usuarios", condicion, datos);
                MessageBox.Show("El usuario se ha actualizado correctamente.");

                LimpiarCampos();
                CargarUsuarios();
            }
            else
            {
                MessageBox.Show("Selecciona un usuario para modificar.", "Información");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string codigo = txtCodigo.Text;

                string condicion = "Codigo = '" + codigo + "'";

                conexion.EliminarRegistro("Usuarios", condicion);
                MessageBox.Show("El usuario se ha eliminado correctamente.");


                LimpiarCampos();
                CargarUsuarios();
            }
            else
            {
                MessageBox.Show("Selecciona un usuario para eliminar.", "Información");
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string nombre = txtBuscar.Text;

            DataTable dt = conexion.BuscarRegistros("Usuarios", "Codigo LIKE '%" + nombre + "%' OR NombreUsuario LIKE '%" + nombre + "%' OR CorreoElectronico LIKE '%" + nombre + "%'");
            dataGridView1.DataSource = dt;
        }

        private void btnTraerDatos_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string codigo = dataGridView1.SelectedRows[0].Cells["Codigo"].Value.ToString();

                conexion.AbrirConexion();

                string query = "SELECT Codigo, NombreUsuario, Nombres, Apellidos, CorreoElectronico,Contraseña " +
                               "FROM Usuarios WHERE Codigo = @codigo";

                OleDbCommand command = new OleDbCommand(query, conexion.con);
                command.Parameters.AddWithValue("@codigo", codigo);

                OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    txtCodigo.Text = dataTable.Rows[0]["Codigo"].ToString();
                    txtMarca.Text = dataTable.Rows[0]["NombreUsuario"].ToString();
                    txtModelo.Text = dataTable.Rows[0]["Nombres"].ToString();
                    txtAnioFabricacion.Text = dataTable.Rows[0]["Apellidos"].ToString();
                    txtPrecioPorDia.Text = dataTable.Rows[0]["CorreoElectronico"].ToString();
                    txtNit.Text = dataTable.Rows[0]["Contraseña"].ToString();
                }
                else
                {
                    MessageBox.Show("No se encontraron datos del usuario seleccionado.");
                }

                conexion.CerrarConexion();
            }
            else
            {
                MessageBox.Show("Seleccione un usuario para traer los datos.");
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
