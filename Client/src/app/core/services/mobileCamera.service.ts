import { Observable, Observer } from 'rxjs/Rx';

import { AbstractCameraService } from './camera.service';

declare let window: any;

export class MobileCameraService implements AbstractCameraService {

    public getPhoto = (): Observable<string> => {
        return Observable.create((observer: Observer<string>) => {
            let removeDomListener = () => {
                document.removeEventListener('deviceready', onCordovaDeviceReady);
            };

            let onCordovaDeviceReady = () => {
                const camera = window.navigator.camera;

                let options = {
                    quality: 100,
                    destinationType: camera.DestinationType.DATA_URL,
                    sourceType: camera.PictureSourceType.CAMERA,
                    encodingType: camera.EncodingType.PNG,
                    pictureSourceType: camera.PictureSourceType.CAMERA,
                    saveToPhotoAlbum: false,
                    targetWidth: 640,
                    targetHeight: 640,
                    correctOrientation: true
                };

                // let options = {
                //     quality: 100,
                //     destinationType: camera.DestinationType.DATA_URL,
                //     sourceType: camera.PictureSourceType.CAMERA,
                //     allowEdit: true,
                //     encodingType: camera.EncodingType.PNG,
                //     saveToPhotoAlbum: false,
                //     correctOrientation: true
                // };

                camera.getPicture((imageData: any) => {
                    observer.next('data:image/png;base64,' + imageData);
                    removeDomListener();
                    observer.complete();
                }, (error: any) => {
                    observer.error(error);
                    removeDomListener();
                    observer.complete();
                }, options);
            };

            document.addEventListener('deviceready', onCordovaDeviceReady);
        });
    }
}
