using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Crm.Dto {

    public class SazemanDto {

        public long Id { get; set; }

        [Required]
        public string SazemanName { get; set; }
        public IList<SemateSazemanDto> SemateSazemen { get; set; } = new List<SemateSazemanDto>();

        public IList<ContactDto> Contacts { get; set; } = new List<ContactDto>();


        // jhipster-needle-dto-add-field - JHipster will add fields here, do not remove
    }
}
