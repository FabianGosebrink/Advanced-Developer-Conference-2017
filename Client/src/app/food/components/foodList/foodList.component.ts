import { Sorter } from '../../../core/services/sort.service';
import { FoodItem } from './../../../shared/models/foodItem.model';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
    selector: 'foodlist',
    templateUrl: './foodList.component.html'
})

export class FoodListComponent {
    foodItem: FoodItem;
    foodToDelete: FoodItem;
    searchString: string;

    @Input() foods: FoodItem[];
    @Output() foodSelected = new EventEmitter<FoodItem>();
    @Output() foodDeleted = new EventEmitter<FoodItem>();

    constructor(private sorter: Sorter) { }

    setFoodToDelete = (foodItem: FoodItem): void => {
        this.foodToDelete = foodItem;
    }

    sortArray(key: string, $event: any) {
        if ($event) {
            $event.preventDefault();
        }
        this.sorter.sort(key, this.foods);
    }

    identify(index: number, item: FoodItem) {
        return item.id;
    }
}
