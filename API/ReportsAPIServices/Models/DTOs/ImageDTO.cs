using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportsAPIServices.Entities;

namespace ReportsAPIServices.Models.DTOs;

public class ImageDTO
{
    public int ReportId { get; set; }
    public string Path { get; set; }
}
