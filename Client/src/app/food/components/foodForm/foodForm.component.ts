import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';

import { FoodItem } from './../../../shared/models/foodItem.model';

@Component({
    selector: 'app-food-form-component',
    templateUrl: './foodForm.component.html'
})

export class FoodFormComponent implements OnChanges {

    types: string[] = ['Starter', 'Main', 'Dessert'];
    @Input() foodItem: FoodItem;
    @Output() foodUpdated = new EventEmitter<FoodItem>();
    @Output() foodAdded = new EventEmitter<FoodItem>();

    currentFood: FoodItem = new FoodItem();

    addOrUpdateFood() {
        this.foodItem.id ? this.foodUpdated.emit(this.currentFood) : this.foodAdded.emit(this.currentFood);
    }

    public ngOnChanges(changes: SimpleChanges): void {
        this.currentFood = Object.assign({}, changes.foodItem.currentValue);
    }
}
