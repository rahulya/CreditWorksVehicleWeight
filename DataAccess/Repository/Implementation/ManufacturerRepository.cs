using DataAccess.DataContext;
using DataAccess.Models;
using DataAccess.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repository.Implementation
{
    public sealed class ManufacturerRepository(AppDbContext context) : EfRepository<Manufacturer>(context), IManufacturerRepository 
    {

        // Add any additional methods specific to Manufacturer repository if needed

    }

}
