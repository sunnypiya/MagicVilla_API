﻿using MagicVilla_API.Models.DTO;

namespace MagicVilla_API.Data
{
    public static class VillaStore
    {
        public static List<VillaDTO> villsList = new List<VillaDTO>()
        {
             new VillaDTO {Id=1,Name="Pool View"},
             new VillaDTO {Id=2,Name="Beach View"}
        };
    }
}