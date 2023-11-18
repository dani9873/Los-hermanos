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
    public partial class FormDevolucionAutomoviles : Form
    {
        private Conexion conexion;
        public FormDevolucionAutomoviles()
        {
            InitializeComponent();
            conexion = new Conexion();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = txtBuscar.Text;

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

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string codigo = txtCodigo.Text;
                string Nombres = comboBoxA.Text;
                string Apellidos = comboBoxC.Text;
                DateTime fechaInicio = dtInicio.Value;
                DateTime fechaDevolucion = dtDevolucion.Value;
                string CorreoElectronico = txtNit.Text;
                string Contraseña = txtNombre.Text;

                Dictionary<string, object> datos = new Dictionary<string, object>();
                datos.Add("FechaInicio", fechaInicio);
                datos.Add("FechaDevolucion", fechaDevolucion);
                datos.Add("CodigoAutomovil", Nombres);
                datos.Add("CodigoCliente", Apellidos);
                datos.Add("NITCliente", CorreoElectronico);
                datos.Add("NombreCliente", Contraseña);
                try
                {
                    string condicion = "CodigoAlquiler = '" + codigo + "'";
                
                    conexion.ModificarRegistro("Alquileres", condicion, datos);
                // Actualizar disponibilidad del automóvil en la tabla Automoviles
                Dictionary<string, object> datosAutomovil = new Dictionary<string, object>();
                datosAutomovil.Add("Disponibilidad", true);

                string condicionAutomovil = "Codigo = @codigo";
                datosAutomovil.Add("Codigo", Nombres);

                conexion.ModificarRegistro("Automoviles", condicionAutomovil, datosAutomovil);

                MessageBox.Show("El alquiler se ha actualizado correctamente.");
                CargarAlquiler();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al insertar el registro en la tabla Alquileres: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Selecciona un automóvil para modificar.", "Información");
            }
        }
        private void CargarAlquiler()
        {
            comboBoxC.DisplayMember = "CodigoAlquiler";
            DataTable dt = conexion.ObtenerRegistros("Alquileres");
            dataGridView1.DataSource = dt;
            comboBoxC.DataSource = dt;
        }

            private void btnTraerDatos_Click(object sender, EventArgs e)
        {
           
                try
                {
                    // Verificar si se ha seleccionado una fila en el DataGridView
                    if (dataGridView1.SelectedRows.Count > 0)
                    {
                        // Obtener el código del cliente seleccionado
                        string codigoAlquiler = dataGridView1.SelectedRows[0].Cells["CodigoAlquiler"].Value.ToString();

                        // Abrir la conexión a la base de datos
                        conexion.AbrirConexion();

                        // Consulta para obtener los datos del cliente seleccionado
                        string query = "SELECT CodigoAlquiler, FechaInicio, FechaDevolucion, CodigoAutomovil, CodigoCliente, NITCliente, NombreCLiente " +
                                       "FROM Alquileres WHERE CodigoAlquiler = @codigoalquiler";

                        // Crear un comando con la consulta y los parámetros
                        OleDbCommand command = new OleDbCommand(query, conexion.con);
                        command.Parameters.AddWithValue("@codigoalquiler", codigoAlquiler);

                        // Crear un adaptador de datos y un DataTable
                        OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                        DataTable dataTable = new DataTable();

                        // Llenar el DataTable con los datos obtenidos de la consulta
                        adapter.Fill(dataTable);

                        // Verificar si se encontraron datos
                        if (dataTable.Rows.Count > 0)
                        {
                            // Obtener los valores de los campos y asignarlos a los TextBox
                            comboBoxA.Text = dataTable.Rows[0]["CodigoAutomovil"].ToString();
                            comboBoxC.Text = dataTable.Rows[0]["CodigoCliente"].ToString();
                            dtInicio.Text = ((DateTime)dataTable.Rows[0]["FechaInicio"]).ToString("yyyy-MM-dd");
                        txtCodigo.Text = dataTable.Rows[0]["CodigoAlquiler"].ToString();
                            txtNombre.Text = dataTable.Rows[0]["NombreCliente"].ToString();
                            txtNit.Text = dataTable.Rows[0]["NITCliente"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("No se encontraron datos del alquiler seleccionado.", "Sin Datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        // Cerrar la conexión a la base de datos
                        conexion.CerrarConexion();
                    }
                    else
                    {
                        MessageBox.Show("Seleccione un alquiler para traer los datos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al obtener los datos del alquiler: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            
        }

        private void FormDevolucionAutomoviles_Load(object sender, EventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormMenuPrincipal formMenuPrincipal = new FormMenuPrincipal();
            formMenuPrincipal.ShowDialog();
            this.Close();
        }
    }
}
