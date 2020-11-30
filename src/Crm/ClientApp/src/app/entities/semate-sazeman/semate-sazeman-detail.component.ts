import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { ISemateSazeman } from 'app/shared/model/semate-sazeman.model';

@Component({
  selector: 'jhi-semate-sazeman-detail',
  templateUrl: './semate-sazeman-detail.component.html',
})
export class SemateSazemanDetailComponent implements OnInit {
  semateSazeman: ISemateSazeman | null = null;

  constructor(protected activatedRoute: ActivatedRoute) {}

  ngOnInit(): void {
    this.activatedRoute.data.subscribe(({ semateSazeman }) => (this.semateSazeman = semateSazeman));
  }

  previousState(): void {
    window.history.back();
  }
}
