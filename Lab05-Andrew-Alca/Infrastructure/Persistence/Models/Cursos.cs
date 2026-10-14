using System;
using System.Collections.Generic;

namespace Lab05_Andrew_Alca.Models;

public partial class Cursos
{
    public int IdCurso { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int Creditos { get; set; }

    public int? IdProfesor { get; set; }

    public virtual ICollection<Asistencias> Asistencias { get; set; } = new List<Asistencias>();

    public virtual ICollection<Evaluaciones> Evaluaciones { get; set; } = new List<Evaluaciones>();

    public virtual Profesores? IdProfesorNavigation { get; set; }

    public virtual ICollection<Materias> Materias { get; set; } = new List<Materias>();

    public virtual ICollection<Matriculas> Matriculas { get; set; } = new List<Matriculas>();
}
