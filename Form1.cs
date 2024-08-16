using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Inventario
{
    public partial class Form1 : Form
    {
        List<Estudiantes> estudiantes;
        public Form1()
        {
            InitializeComponent();
            estudiantes = new List<Estudiantes>();
            eliminarToolStripMenuItem.Enabled = false;
        }
        private void registrarToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (!ValidarCampos())
            {
                return;
            }
            Estudiantes nuevoEstudiante = new Estudiantes(txtIdentificacion.Text, txtNombreEstudiante.Text,
                int.Parse(txtEdad.Text), int.Parse(cmbEstrato.SelectedItem.ToString()), cmbPrograma.SelectedItem.ToString(),
                cmbUniversidad.SelectedItem.ToString(), dtpFechaRegistro.Value);
            estudiantes.Add(nuevoEstudiante);// Se agrega el nuevo estudiante en la lista.

            string connectionString = "Server=LAPTOP-PL8Q7BP5;Database=EstudiantesDB;User Id=sa;Password=123;";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "INSERT INTO Estudiantes (Identificacion, Nombre, Universidad, Edad, Estrato, Programa,FechaRegistro) " +
                    "VALUES (@Identificacion, @Nombre, @Universidad, @Edad, @Estrato, @Programa,@FechaRegistro)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Identificacion", nuevoEstudiante.IdentificacionEstudiante);
                    cmd.Parameters.AddWithValue("@Nombre", nuevoEstudiante.Nombre);
                    cmd.Parameters.AddWithValue("@Universidad", nuevoEstudiante.Universidad);
                    cmd.Parameters.AddWithValue("@Edad", nuevoEstudiante.Edad);
                    cmd.Parameters.AddWithValue("@Estrato", nuevoEstudiante.Estrato);
                    cmd.Parameters.AddWithValue("@Programa", nuevoEstudiante.Programa);
                    cmd.Parameters.AddWithValue("@FechaRegistro", nuevoEstudiante.FechaRegistro);

                    cmd.ExecuteNonQuery();
                }
            }


            MostrarDatosEnData();
            LimpiarCampos();
        }
        private void MostrarDatosEnData()
        {
            string connectionString = "Server=LAPTOP-PL8Q7BP5;Database=EstudiantesDB;User Id=sa;Password=123;";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT Identificacion, Nombre, Universidad, Edad, Estrato, Programa, FechaRegistro FROM Estudiantes";
                using (SqlDataAdapter da = new SqlDataAdapter(query, con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = dt;
                }
            }
            //dataGridView1.DataSource = null;
            //dataGridView1.DataSource = estudiantes.ToArray();
        }

        private void LimpiarCampos()
        {
            txtIdentificacion.Text = string.Empty;
            txtNombreEstudiante.Text = string.Empty;
            txtEdad.Text = string.Empty;
            cmbUniversidad.SelectedIndex = -1;
            cmbPrograma.SelectedIndex = -1;
            cmbEstrato.SelectedIndex = -1;
            dtpFechaRegistro.Value = DateTime.Now;
        }
        private bool CamposLlenos()
        {
            if (string.IsNullOrWhiteSpace(txtIdentificacion.Text) || string.IsNullOrWhiteSpace(txtNombreEstudiante.Text) ||
                string.IsNullOrWhiteSpace(txtEdad.Text) || string.IsNullOrWhiteSpace(cmbEstrato.Text) ||
                string.IsNullOrWhiteSpace(cmbPrograma.Text) || string.IsNullOrWhiteSpace(cmbUniversidad.Text))
            {
                MessageBox.Show("Todos los campos deben ir obligatoriamente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private bool ValidarCampos()
        {
            if (!CamposLlenos())
            {
                return false;
            }
            if (!EsNumero(txtIdentificacion.Text))
            {
                MessageBox.Show("El numero de identificacion debe ser numerico.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!EsTexto(txtNombreEstudiante.Text))
            {
                MessageBox.Show("El Nombre del estudiante debe ser de cadena de caracteres.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;

        }
        private bool EsTexto(string texto)
        {
            foreach (char c in texto)
            {

                if (!char.IsLetter(c) && !char.IsWhiteSpace(c))
                {
                    return false;
                }

            }
            return true;
        }
        private bool EsNumero(string texto)
        {
            foreach (char c in texto)
            {

                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        private void eliminarToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (estudiantes.Count == 0)
            {
                MessageBox.Show("No se encontraron registros para eliminar", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string identificacionEliminar = txtIdentificacion.Text.Trim();
            Estudiantes estudianteEliminar = estudiantes.FirstOrDefault(est => est.IdentificacionEstudiante == identificacionEliminar);

            if (estudianteEliminar != null)
            {
                DialogResult result = MessageBox.Show("¿Está seguro de que desea eliminar al estudiante?\n\n" +
                    "Identificación: " + estudianteEliminar.IdentificacionEstudiante + "\n" +
                    "Nombre: " + estudianteEliminar.Nombre + "\n" + "Universidad: " + estudianteEliminar.Universidad,
                    "Programa: " + estudianteEliminar.Programa +
                    "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question
                );
                if (result == DialogResult.Yes)
                {
                    // Eliminar de la lista
                    estudiantes.Remove(estudianteEliminar);

                    // Eliminar de la base de datos
                    string connectionString = "Server=LAPTOP-PL8Q7BP5;Database=EstudiantesDB;User Id=sa;Password=123;";
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                       /* con.Open();
                        string query = "DELETE FROM Estudiantes WHERE Identificacion = @Identificacion";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@Identificacion", estudianteEliminar.IdentificacionEstudiante);
                            cmd.ExecuteNonQuery();
                        }*/
                        con.Open();
                        SqlTransaction transaction = con.BeginTransaction();

                        try
                        {
                            string query = "DELETE FROM Estudiantes WHERE Identificacion = @Identificacion";
                            using (SqlCommand cmd = new SqlCommand(query, con, transaction))
                            {
                                cmd.Parameters.AddWithValue("@Identificacion", estudianteEliminar.IdentificacionEstudiante);
                                cmd.ExecuteNonQuery();
                            }

                            // Commit the transaction if everything is fine
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            // Rollback the transaction in case of error
                            transaction.Rollback();
                            MessageBox.Show("Error eliminando el estudiante: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }


                    // Actualizar el DataGridView
                    MostrarDatosEnData();

                    eliminarToolStripMenuItem.Enabled = false;
                    LimpiarCampos();
                    MessageBox.Show("El estudiante ha sido eliminado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("El estudiante no se encuentra en la lista.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void consulToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string id = txtIdentificacion.Text;
            if (existeEstudiante(id))
            {
                eliminarToolStripMenuItem.Enabled = true; // Habilitar botón de eliminar si el estudiante existe
            }
            else
            {
                eliminarToolStripMenuItem.Enabled = false; // Deshabilitar botón si no existe
            }

        }
        private bool existeEstudiante(string id)
        {
            string connectionString = "Server=LAPTOP-PL8Q7BP5;Database=EstudiantesDB;User Id=sa;Password=123;";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT Identificacion, Nombre, Universidad, Edad, Estrato, Programa, FechaRegistro FROM Estudiantes WHERE Identificacion = @Identificacion";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Identificacion", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Cargar datos del estudiante en los campos del formulario
                            DateTime fechaRegistro = reader.IsDBNull(reader.GetOrdinal("FechaRegistro")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FechaRegistro"));

                            Estudiantes estudianteEncontrado = new Estudiantes(
                                reader["Identificacion"].ToString(),
                                reader["Nombre"].ToString(),
                                reader.GetInt32(reader.GetOrdinal("Edad")),
                                reader.GetInt32(reader.GetOrdinal("Estrato")),
                                reader["Programa"].ToString(),
                                reader["Universidad"].ToString(),
                                fechaRegistro
                            );
                            cargarDatosAEliminar(estudianteEncontrado);
                            return true;
                        }
                    }
                }
            }

            MessageBox.Show("El estudiante no se encuentra en la base de datos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
            /*if (estudiantes.Count == 0)
            {
                return false;
            }
           */
        }
        private void cargarDatosAEliminar(Estudiantes estudiante)
        {
            txtIdentificacion.Text = estudiante.IdentificacionEstudiante;
            txtNombreEstudiante.Text = estudiante.Nombre;
            txtEdad.Text = estudiante.Edad.ToString();
            cmbEstrato.Text = estudiante.Estrato.ToString();
            cmbPrograma.Text = estudiante.Programa.ToString();
            cmbUniversidad.Text = estudiante.Universidad.ToString();
            dtpFechaRegistro.Text = estudiante.FechaRegistro.ToString();
        }

        private void limpiarCamposToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }
    }
}

