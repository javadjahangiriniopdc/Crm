using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Crm.Dto {

    public class SemateSazemanDto {

        public long Id { get; set; }

        [Required]
        public string SemateSazemanName { get; set; }
        public IList<ContactDto> Contacts { get; set; } = new List<ContactDto>();

        public SazemanDto Sazeman { get; set; }


        // jhipster-needle-dto-add-field - JHipster will add fields here, do not remove
    }
}
