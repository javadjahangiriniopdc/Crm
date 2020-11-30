import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { ISazeman } from 'app/shared/model/sazeman.model';

@Component({
  selector: 'jhi-sazeman-detail',
  templateUrl: './sazeman-detail.component.html',
})
export class SazemanDetailComponent implements OnInit {
  sazeman: ISazeman | null = null;

  constructor(protected activatedRoute: ActivatedRoute) {}

  ngOnInit(): void {
    this.activatedRoute.data.subscribe(({ sazeman }) => (this.sazeman = sazeman));
  }

  previousState(): void {
    window.history.back();
  }
}
