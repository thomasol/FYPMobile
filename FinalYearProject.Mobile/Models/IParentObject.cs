using System;
using System.Collections.Generic;

namespace FinalYearProject.Mobile.Models
{
    public interface IParentObject
    {
        List<Object> ChildObjectList { get; set; }
    }
}
