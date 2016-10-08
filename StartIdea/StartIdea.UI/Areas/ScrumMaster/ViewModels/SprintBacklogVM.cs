﻿using PagedList;
using StartIdea.Model.ScrumArtefatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StartIdea.UI.Areas.ScrumMaster.ViewModels
{
    public class SprintBacklogVM
    {
        public int paginaProductBacklog { get; set; }
        public IPagedList<ProductBacklog> ProductBacklog { get; set; }

        public int paginaSprintBacklog { get; set; }
        public IPagedList<ProductBacklog> SprintBacklog { get; set; }

        public string DisplayMotivoCancelamento { get; set; }
        public int Id { get; set; }
        public string MotivoCancelamento { get; set; }
    }
}