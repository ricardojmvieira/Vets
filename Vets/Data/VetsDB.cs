﻿using Vets.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vets.Data
{
    /// <summary>
    /// classe para representar a base de dados VeterinariosDB
    /// </summary>
    public class VetsDB : DbContext
    {
        /// <summary>
        /// Construtor da classe
        /// serve para ligar esta classe à BD
        /// </summary>
        /// <param name="options"></param>
        public VetsDB(DbContextOptions<VetsDB> options) : base(options) { }

        //adicionar as 'tabelas' à BD
        public DbSet<Animais> Animais { get; set; }
        public DbSet<Donos> Donos { get; set; }
        public DbSet<Veterinarios> Veterinarios { get; set; }
        public DbSet<Consultas> Consultas { get; set; }
    }
}

