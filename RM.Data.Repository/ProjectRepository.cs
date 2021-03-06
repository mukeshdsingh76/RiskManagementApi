﻿using Microsoft.EntityFrameworkCore;
using RM.Data.Models;
using RM.Data.Repository.Contract;
using System.Threading.Tasks;

namespace RM.Data.Repository
{
  public class ProjectRepository : Repository<Project>, IProjectRepository
  {
    private readonly RMContext _context;

    public ProjectRepository(RMContext context) : base(context)
    {
      _context = context;
    }

    public Task<Project> GetProjectWithProjectStatus(int id)
    {
      return _context.Set<Project>().Include(task => task.ProjectStatus).FirstOrDefaultAsync(x => x.Id == id);
    }
  }
}