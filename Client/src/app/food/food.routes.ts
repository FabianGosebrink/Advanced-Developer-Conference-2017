import { Routes } from '@angular/router';

import { AuthGuard } from '../shared/guards/authentication.guard';
import { FoodDetailsComponent } from './components/foodDetails/foodDetails.component';
import { MainFoodComponent } from './components/mainFood/mainFood.component';


export const FoodRoutes: Routes = [
    {
        path: '', component: MainFoodComponent, canActivate: [AuthGuard]
    },
    {
        path: ':foodId', component: FoodDetailsComponent, canActivate: [AuthGuard]
    }
];
