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
    public partial class FormMantenimientoAutomoviles : Form
    {
        private Conexion conexion;

        public FormMantenimientoAutomoviles()
        {
            InitializeComponent();
            conexion = new Conexion();
        }


        private void CargarAutomoviles()
        {
            DataTable dt = conexion.ObtenerRegistros("Automoviles");
            dataGridView1.DataSource = dt;
            dataGridView1.Columns["PrecioPorDia"].DefaultCellStyle.Format = "C2";
            dataGridView1.Columns["PrecioPorDia"].DefaultCellStyle.FormatProvider = CultureInfo.GetCultureInfo("es-GT");
        }


        private void LimpiarCampos()
        {
            txtCodigo.Text = string.Empty;
            txtMarca.Text = string.Empty;
            txtModelo.Text = string.Empty;
            txtAnioFabricacion.Text = string.Empty;
            txtPrecioPorDia.Text = string.Empty;
            chkDisponibilidad.Checked = false;
        }

        private void FormMantenimientoAutomoviles_Load(object sender, EventArgs e)
        {
            CargarAutomoviles();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                string codigo = txtCodigo.Text;
            // Verificar la condición de la identificación
            if (!codigo.StartsWith("A") || !codigo.Substring(1).All(char.IsDigit))
            {
                MessageBox.Show("El código debe comenzar con 'A' seguido de números.");
                return;
            }
            string marca = txtMarca.Text;
            string modelo = txtModelo.Text;
            string anioFabricacion = txtAnioFabricacion.Text;
            decimal precioPorDia = decimal.Parse(txtPrecioPorDia.Text);
            bool disponibilidad = chkDisponibilidad.Checked;

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
            datos.Add("Marca", marca);
            datos.Add("Modelo", modelo);
            datos.Add("AnioFabricacion", anioFabricacion);
            datos.Add("PrecioPorDia", precioPorDia);
            datos.Add("Disponibilidad", disponibilidad);

            conexion.InsertarRegistro("Automoviles", datos);

            LimpiarCampos();
            CargarAutomoviles();
            MessageBox.Show("El automóvil se ha insertado correctamente.");

            }
            catch (Exception ex)
    {
        MessageBox.Show("Error al insertar el automóvil: " + ex.Message);
        MessageBox.Show("Error al insertar el automóvil: " + ex.Message);
    }
}

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string codigo = txtCodigo.Text;
                string marca = txtMarca.Text;
                string modelo = txtModelo.Text;
                string anioFabricacion = txtAnioFabricacion.Text;
                decimal precioPorDia = decimal.Parse(txtPrecioPorDia.Text);
                bool disponibilidad = chkDisponibilidad.Checked;

                Dictionary<string, object> datos = new Dictionary<string, object>();
                datos.Add("Marca", marca);
                datos.Add("Modelo", modelo);
                datos.Add("AnioFabricacion", anioFabricacion);
                datos.Add("PrecioPorDia", precioPorDia);
                datos.Add("Disponibilidad", disponibilidad);

                string condicion = "Codigo = '" + codigo + "'";

                conexion.ModificarRegistro("Automoviles", condicion, datos);
                MessageBox.Show("El automóvil se ha actualizado correctamente.");

                LimpiarCampos();
                CargarAutomoviles();
            }
            else
            {
                MessageBox.Show("Selecciona un automóvil para modificar.", "Información");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string codigo = txtCodigo.Text;

                string condicion = "Codigo = '" + codigo + "'";

                conexion.EliminarRegistro("Automoviles", condicion);
                MessageBox.Show("El automóvil se ha eliminado correctamente.");

                LimpiarCampos();
                CargarAutomoviles();
            }
            else
            {
                MessageBox.Show("Selecciona un automóvil para eliminar.", "Información");
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string nombre = txtBuscar.Text;

            DataTable dt = conexion.BuscarRegistros("Automoviles", "Codigo LIKE '%" + nombre + "%' OR Marca LIKE '%" + nombre + "%' OR Modelo LIKE '%" + nombre + "%'");
            dataGridView1.DataSource = dt;
        }

        private void btnTraerDatos_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string codigo = dataGridView1.SelectedRows[0].Cells["Codigo"].Value.ToString();

                conexion.AbrirConexion();

                string query = "SELECT Codigo, Marca, Modelo, AnioFabricacion, PrecioPorDia, Disponibilidad " +
                               "FROM Automoviles WHERE Codigo = @codigo";

                OleDbCommand command = new OleDbCommand(query, conexion.con);
                command.Parameters.AddWithValue("@codigo", codigo);

                OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    txtCodigo.Text = dataTable.Rows[0]["Codigo"].ToString();
                    txtMarca.Text = dataTable.Rows[0]["Marca"].ToString();
                    txtModelo.Text = dataTable.Rows[0]["Modelo"].ToString();
                    txtAnioFabricacion.Text = dataTable.Rows[0]["AnioFabricacion"].ToString();
                    txtPrecioPorDia.Text = dataTable.Rows[0]["PrecioPorDia"].ToString();
                    chkDisponibilidad.Checked = (bool)dataTable.Rows[0]["Disponibilidad"];
                }
                else
                {
                    MessageBox.Show("No se encontraron datos del automóvil seleccionado.");
                }

                conexion.CerrarConexion();
            }
            else
            {
                MessageBox.Show("Seleccione un automóvil para traer los datos.");
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
