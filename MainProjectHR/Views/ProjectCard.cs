using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProjectHR.Views
{
    public class ProjectCard
    {
        public string ProjectImage { get; set; } // Путь к изображению
        public string ProjectName { get; set; } // Название проекта
        public List<string> Workers { get; set; } // Список сотрудников
    }
}
