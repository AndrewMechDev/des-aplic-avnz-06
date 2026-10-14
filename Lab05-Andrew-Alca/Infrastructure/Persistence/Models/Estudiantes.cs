using System;
using System.Collections.Generic;

namespace Lab05_Andrew_Alca.Models;

public partial class Estudiantes
{
    public int IdEstudiante { get; set; }

    public string Nombre { get; set; } = null!;

    public int Edad { get; set; }

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public virtual ICollection<Asistencias> Asistencias { get; set; } = new List<Asistencias>();

    public virtual ICollection<Evaluaciones> Evaluaciones { get; set; } = new List<Evaluaciones>();

    public virtual ICollection<Matriculas> Matriculas { get; set; } = new List<Matriculas>();
}
