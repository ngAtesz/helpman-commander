import { DataSource } from '@angular/cdk/collections';
import { MatPaginator, MatSort } from '@angular/material';
import { map } from 'rxjs/operators';
import { Observable, of as observableOf, merge } from 'rxjs';

// TODO: Replace this with your own data model type
export interface CompetitionListItem {
  id: number;
  category: string;
  date: Date;
  finalized: boolean;
}

// TODO: replace this with real data from your application
const EXAMPLE_DATA: CompetitionListItem[] = [
  {id: 1, category: 'Gyermek', date: new Date('2018-10-23'), finalized: true},
  {id: 2, category: 'Gyermek', date: new Date('2018-9-23'), finalized: true},
  {id: 3, category: 'Gyermek', date: new Date('2018-5-23'), finalized: true},
  {id: 4, category: 'Gyermek', date: new Date('2017-10-23'), finalized: true},
  {id: 5, category: 'Gyermek', date: new Date('2019-10-23'), finalized: false},
  {id: 6, category: 'Gyermek', date: new Date('2019-9-2'), finalized: false},
];

/**
 * Data source for the CompetitionList view. This class should
 * encapsulate all logic for fetching and manipulating the displayed data
 * (including sorting, pagination, and filtering).
 */
export class CompetitionListDataSource extends DataSource<CompetitionListItem> {
  data: CompetitionListItem[] = EXAMPLE_DATA;
  paginator: MatPaginator;
  sort: MatSort;

  constructor() {
    super();
  }

  /**
   * Connect this data source to the table. The table will only update when
   * the returned stream emits new items.
   * @returns A stream of the items to be rendered.
   */
  connect(): Observable<CompetitionListItem[]> {
    // Combine everything that affects the rendered data into one update
    // stream for the data-table to consume.
    const dataMutations = [
      observableOf(this.data),
      this.paginator.page,
      this.sort.sortChange
    ];

    return merge(...dataMutations).pipe(map(() => {
      return this.getPagedData(this.getSortedData([...this.data]));
    }));
  }

  /**
   *  Called when the table is being destroyed. Use this function, to clean up
   * any open connections or free any held resources that were set up during connect.
   */
  disconnect() {}

  /**
   * Paginate the data (client-side). If you're using server-side pagination,
   * this would be replaced by requesting the appropriate data from the server.
   */
  private getPagedData(data: CompetitionListItem[]) {
    const startIndex = this.paginator.pageIndex * this.paginator.pageSize;
    return data.splice(startIndex, this.paginator.pageSize);
  }

  /**
   * Sort the data (client-side). If you're using server-side sorting,
   * this would be replaced by requesting the appropriate data from the server.
   */
  private getSortedData(data: CompetitionListItem[]) {
    if (!this.sort.active || this.sort.direction === '') {
      return data;
    }

    return data.sort((a, b) => {
      const isAsc = this.sort.direction === 'asc';
      switch (this.sort.active) {
        case 'category': return compare(a.category, b.category, isAsc);
        case 'year': return compare(a.date.getFullYear(), b.date.getFullYear(), isAsc);
        case 'date': return compare(a.date.getUTCDate(), b.date.getUTCDate(), isAsc);
        default: return 0;
      }
    });
  }
}

/** Simple sort comparator for example ID/Name columns (for client-side sorting). */
function compare(a, b, isAsc) {
  return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
}
