import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BorrowedTransactionsComponent } from './borrowed-transactions.component';

describe('BorrowedTransactionsComponent', () => {
  let component: BorrowedTransactionsComponent;
  let fixture: ComponentFixture<BorrowedTransactionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BorrowedTransactionsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BorrowedTransactionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
