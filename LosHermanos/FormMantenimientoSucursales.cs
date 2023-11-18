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
    public partial class FormMantenimientoSucursales : Form
    {
        private Conexion conexion;

        public FormMantenimientoSucursales()
        {
            InitializeComponent();
            conexion = new Conexion();
        }
        private void CargarSucursales()
        {
            DataTable dt = conexion.ObtenerRegistros("Sucursales");
            dataGridView1.DataSource = dt;
        }


        private void LimpiarCampos()
        {
            txtCodigo.Text = string.Empty;
            txtMarca.Text = string.Empty;
            txtModelo.Text = string.Empty;
            txtAnioFabricacion.Text = string.Empty;
        }


        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
            string codigo = txtCodigo.Text;
            // Verificar la condición de la identificación
            if (!codigo.StartsWith("S") || !codigo.Substring(1).All(char.IsDigit))
            {
                MessageBox.Show("El código debe comenzar con 'S' seguido de números.");
                return;
            }
            string Nombre = txtMarca.Text;
            string Direccion = txtModelo.Text;
            string CapacidadAlmacenamiento = txtAnioFabricacion.Text;


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
            datos.Add("Nombre", Nombre);
            datos.Add("Direccion", Direccion);
            datos.Add("CapacidadAlmacenamiento", CapacidadAlmacenamiento);


            conexion.InsertarRegistro("Sucursales", datos);

            LimpiarCampos();
            CargarSucursales();
            MessageBox.Show( "La sucursal se ha insertado correctamente.");
        }
    catch (Exception ex)
    {
        MessageBox.Show("Error al insertar la sucursal: " + ex.Message);
    }
}

        private void btnModificar_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count > 0)
            {
                string codigo = txtCodigo.Text;
                string Nombre = txtMarca.Text;
                string Direccion = txtModelo.Text;
                string CapacidadAlmacenamiento = txtAnioFabricacion.Text;

                Dictionary<string, object> datos = new Dictionary<string, object>();
                datos.Add("Nombre", Nombre);
                datos.Add("Direccion", Direccion);
                datos.Add("CapacidadAlmacenamiento", CapacidadAlmacenamiento);


                string condicion = "Codigo = '" + codigo + "'";

                conexion.ModificarRegistro("Sucursales", condicion, datos);

                LimpiarCampos();
                CargarSucursales();
                MessageBox.Show("La sucursal se ha actualizado correctamente.");
            }
            else
            {
                MessageBox.Show("Selecciona un sucursal para modificar.", "Información");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
                    if (dataGridView1.SelectedRows.Count > 0)
            {
                string codigo = txtCodigo.Text;

                string condicion = "Codigo = '" + codigo + "'";

                conexion.EliminarRegistro("Sucursales", condicion);
                MessageBox.Show("La sucursal se ha eliminado correctamente.");

                LimpiarCampos();
                CargarSucursales();
            }
            else
            {
                MessageBox.Show("Selecciona un sucursal para eliminar.", "Información");
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string nombre = txtBuscar.Text;

            DataTable dt = conexion.BuscarRegistros("Sucursales", "Codigo LIKE '%" + nombre + "%' OR Nombre LIKE '%" + nombre + "%' OR Direccion LIKE '%" + nombre + "%'");
            dataGridView1.DataSource = dt;
        }

        private void btnTraerDatos_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string codigo = dataGridView1.SelectedRows[0].Cells["Codigo"].Value.ToString();

                conexion.AbrirConexion();

                string query = "SELECT Codigo, Nombre, Direccion, CapacidadAlmacenamiento " +
                               "FROM Sucursales WHERE Codigo = @codigo";

                OleDbCommand command = new OleDbCommand(query, conexion.con);
                command.Parameters.AddWithValue("@codigo", codigo);

                OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);
                
                if (dataTable.Rows.Count > 0)
                {
                    txtCodigo.Text = dataTable.Rows[0]["Codigo"].ToString();
                    txtMarca.Text = dataTable.Rows[0]["Nombre"].ToString();
                    txtModelo.Text = dataTable.Rows[0]["Direccion"].ToString();
                    txtAnioFabricacion.Text = dataTable.Rows[0]["CapacidadAlmacenamiento"].ToString();
                }
                else
                {
                    MessageBox.Show("No se encontraron datos del sucursal seleccionado.");
                }

                conexion.CerrarConexion();
            }
            else
            {
                MessageBox.Show("Seleccione un sucursal para traer los datos.");
            }
        }

        private void FormMantenimientoSucursales_Load(object sender, EventArgs e)
        {
                CargarSucursales();
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



