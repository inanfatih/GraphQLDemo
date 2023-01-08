﻿using GraphQLDemo.API.DTOs;
using GraphQLDemo.API.Models;
using System;

namespace GraphQLDemo.API.Schema.Mutations
{
    public class CourseResult
    {
        public CourseResult()
        {

        }
        public CourseResult(CourseDTO courseDTO)
        {
            Id = courseDTO.Id;
            Name = courseDTO.Name;
            Subject = courseDTO.Subject;
            InstructorId = courseDTO.InstructorId;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Subject Subject { get; set; }
        public Guid InstructorId { get; set; }
    }
}
