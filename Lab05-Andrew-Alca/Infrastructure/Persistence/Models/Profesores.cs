using System;
using System.Collections.Generic;

namespace Lab05_Andrew_Alca.Models;

public partial class Profesores
{
    public int IdProfesor { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Especialidad { get; set; }

    public string? Correo { get; set; }

    public virtual ICollection<Cursos> Cursos { get; set; } = new List<Cursos>();
}
