﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using BangazonWorkforceManagement.Models;
using BangazonWorkforceManagement.Models.ViewModels;

namespace BangazonWorkforceManagement.Controllers
{
    public class TrainingProgramController : Controller
    {
            private readonly IConfiguration _config;

            public TrainingProgramController(IConfiguration config)
            {
                _config = config;
            }

            public SqlConnection Connection
            {
                get
                {
                    return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
                }
            }

        // GET: TrainingProgram
        public ActionResult Index()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        SELECT t.Id,
                                        t.Name,
                                        t.StartDate,
                                        t.EndDate,
                                        t.MaxAttendees
                                    FROM TrainingProgram t
									WHERE t.StartDate > GETDATE();
                                    ";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<TrainingProgram> trainingPrograms = new List<TrainingProgram>();
                    while (reader.Read())
                    {
                        TrainingProgram trainingProgram = new TrainingProgram
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                            EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                            MaxAttendees = reader.GetInt32(reader.GetOrdinal("MaxAttendees"))
                        };

                        trainingPrograms.Add(trainingProgram);
                    }

                    reader.Close();

                    return View(trainingPrograms);
                }
            }
        }

        // GET: TrainingProgram/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TrainingProgram/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TrainingProgram/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TrainingProgram trainingProgram)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO TrainingProgram
                                        (Name, StartDate, EndDate, MaxAttendees)
                                        VALUES ( @name, @startDate, @endDate, @MaxAttendees)";
                    cmd.Parameters.Add(new SqlParameter("@name", trainingProgram.Name));
                    cmd.Parameters.Add(new SqlParameter("@startDate", trainingProgram.StartDate));
                    cmd.Parameters.Add(new SqlParameter("@endDate", trainingProgram.EndDate));
                    cmd.Parameters.Add(new SqlParameter("@maxAttendees", trainingProgram.MaxAttendees));
                    cmd.ExecuteNonQuery();

                    return RedirectToAction(nameof(Index));
                }
            }
        }

        // GET: TrainingProgram/Edit/5
        public ActionResult Edit(int id)
        {
            var trainingProgram = GetTrainingProgramById(id);
            return View(trainingProgram);
        }

        // POST: TrainingProgram/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, TrainingProgram trainingProgram)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = cmd.CommandText = @"Update TrainingProgram 
                                                              SET Name = @name, 
                                                                   StartDate = @startDate,
                                                                    EndDate = @endDate,
                                                                    MaxAttendees = @maxAttendees
                                                              WHERE id = @id";
                        cmd.Parameters.Add(new SqlParameter("@name", trainingProgram.Name));
                        cmd.Parameters.Add(new SqlParameter("@startDate", trainingProgram.StartDate));
                        cmd.Parameters.Add(new SqlParameter("@endDate", trainingProgram.EndDate));
                        cmd.Parameters.Add(new SqlParameter("@maxAttendees", trainingProgram.MaxAttendees));
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        cmd.ExecuteNonQuery();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TrainingProgram/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TrainingProgram/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        private TrainingProgram GetTrainingProgramById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                               SELECT  
                                tp.Id,
                                tp.Name,
                                tp.StartDate,
                                tp.EndDate,
                                tp.MaxAttendees
                                FROM TrainingProgram tp
                                WHERE tp.Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();
                    TrainingProgram trainingProgram = new TrainingProgram();
                    while (reader.Read())
                    {
                        trainingProgram = new TrainingProgram()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                            EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                            MaxAttendees = reader.GetInt32(reader.GetOrdinal("MaxAttendees")),
                        };
                    }
                    reader.Close();
                    return trainingProgram;
                }
            }
        }
        




    }
}