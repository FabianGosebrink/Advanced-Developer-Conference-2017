import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { RouterTestingModule } from '@angular/router/testing';

import { AbstractCameraServiceStub } from '../../../testing/abstractCameraServiceMock';
import { AbstractNotificationServiceStub } from '../../../testing/abstractNotificationServiceMock';
import { CpuValueServiceMock } from '../../../testing/CpuValueServiceMock';
import { FoodDataService } from '../../core/data-services/food-data.service';
import { AbstractCameraService } from '../../core/services/camera.service';
import { CpuValueService } from '../../core/services/cpuValue.service';
import { AbstractNotificationService } from '../../core/services/notification.service';
import { PlatformInformationProvider } from '../../core/services/platformInformation.provider';
import { FoodItem } from '../../shared/models/foodItem.model';
import { EMealFooterComponent } from '../footer/eMeal-footer.component';
import { RandomMealComponent } from '../randomMeal/randomMeal.component';
import { SneakPeekComponent } from '../sneakPeek/sneekPeek.component';
import { FoodServiceMock } from './../../../testing/foodServiceMock';
import { HomeComponent } from './home.component';

describe('HomeComponent', () => {

  let fixture: ComponentFixture<HomeComponent>;
  let comp: HomeComponent;

  // async beforeEachs
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule
      ],
      declarations: [HomeComponent, SneakPeekComponent, RandomMealComponent, EMealFooterComponent],
      providers: [
        { provide: FoodDataService, useClass: FoodServiceMock },
        { provide: AbstractNotificationService, useClass: AbstractNotificationServiceStub },
        { provide: CpuValueService, useClass: CpuValueServiceMock },
        { provide: AbstractCameraService, useClass: AbstractCameraServiceStub },
        PlatformInformationProvider
      ]
    }).compileComponents(); // compile template and css
  }));

  // synchronous beforeEach
  beforeEach(() => {
    fixture = TestBed.createComponent(HomeComponent);
    comp = fixture.componentInstance;
  });

  afterEach(() => {
    fixture.destroy();
  });

  it('component should be instanciated', () => {
    expect(comp).toBeDefined();
  });

  it('updatefood should be defined', () => {
    expect(comp.updateFood).toBeDefined();
  });

  it('updatefood should call service --> getRandomMeal', () => {
    const service = TestBed.get(FoodDataService);
    service.getRandomMeal = jasmine.createSpy('getRandomMeal').and.returnValue(service.getRandomMeal());

    comp.updateFood();

    fixture.whenStable().then(() => {
      expect(service.getRandomMeal).toHaveBeenCalled();
      expect(comp.randomFood).toBeDefined();
      expect(comp.randomFood.length).toEqual(3);
    })
  });

  it('after init was called \'allFood\' is set', () => {
    const foodDataService = TestBed.get(FoodDataService);

    fixture.detectChanges();    // call init here

    expect(comp.allFood).toBeDefined();
    comp.allFood.subscribe((data: FoodItem[]) => {
      expect(data.length).toBeGreaterThanOrEqual(0);
    });
  });

  it('after init was called \'randomFood\' is set', () => {
    const foodDataService = TestBed.get(FoodDataService);

    fixture.detectChanges();    // call init here
    expect(comp.randomFood).toBeDefined();
    expect(comp.randomFood.length).toEqual(3);
  });

  it('selectedFood should be one foodItem', () => {
    const service = TestBed.get(FoodDataService);
    service.getAllFood = jasmine.createSpy('getAllFood').and.returnValue(service.getAllFood());
    comp.updateFood();
    expect(comp.randomFood).toBeDefined();
  });

  it('h2 should give correct headline', () => {
    const de = fixture.debugElement.query(By.css('h2'));
    const el = de.nativeElement;
    expect(el.textContent).toEqual('Your meal for today');
  });
});
