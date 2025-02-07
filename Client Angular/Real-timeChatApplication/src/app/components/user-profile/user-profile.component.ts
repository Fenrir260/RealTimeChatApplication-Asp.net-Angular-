import { Component, OnInit } from '@angular/core';
import { UserDTO } from '../../../DTO/userDTO';
import { ActivatedRoute } from '@angular/router';
import { UserProfile } from '../../../DTO/userProfile';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { UserProfileService } from '../../services/user-profile.service';
import { UsersService } from '../../services/users.service';
import { CookieService } from '../../services/cookie.service';
import { userTokenDTO } from '../../../DTO/userTokenDTO';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css'] 
})
export class UserProfileComponent implements OnInit {
  userInit: UserDTO = {
    userName: '',
    email: '',
    authorized: false,
  };

  userProfile: UserProfile = {
    userId: 0,
    firstName: "",
    lastName: "",
    aboutMe: "",
    age: 0,
    gender: "",
    interestedIn: "",
    instagram: "",
    showInstagram: false,
    telegram: "",
    showTelegram: false,
    photo: '',
    userName: "",
  };

  isEditing: boolean = false;

  constructor(
    private _activatedRoute: ActivatedRoute,
    private _userProfileService: UserProfileService,
    private _userService: UsersService,
    private _cookieService: CookieService
  ) {}

  ngOnInit(): void {
    const urlUserName = this._activatedRoute.snapshot.paramMap.get('userName');
    this._cookieService.getDecodedTokenFromCookie('jwtToken').subscribe({
      next: (response: userTokenDTO) => {
        this.userInit.userName = `${response.userName}`;
        this.userInit.email = response.email;
        this.userInit.authorized = response.authorized === 'true'; 

        this._userService.getIdByUserName(this.userInit.userName).subscribe({
          next: (id) =>{
            this.userProfile.userId = id;
            this.uploadPhotoIntoProfile();
            this.loadUserProfile();
          },
          error: (err) =>{
            console.log(err);
          }
        })
      },
      error: (err) => {
        console.error('Error occurred:', err);
      }
    });
    
    
  }

  toggleEdit(): void {
    this.isEditing = !this.isEditing;
  }

  saveProfile(): void {
    this.isEditing = false;
  
    this._userService.getIdByUserName(this.userInit.userName).subscribe({
      next: (id) => {
        this.userProfile.userId = id; 
        const userProfileJson = JSON.stringify({
          userId: this.userProfile.userId,  
          age: this.userProfile.age,
          gender: this.userProfile.gender,
          aboutMe: this.userProfile.aboutMe,
          lastName: this.userProfile.lastName,
          telegram: this.userProfile.telegram,
          firstName: this.userProfile.firstName,
          instagram: this.userProfile.instagram,
          interestedIn: this.userProfile.interestedIn,
          showTelegram: this.userProfile.showTelegram,
          showInstagram: this.userProfile.showInstagram,
          userName: this.userInit.userName
        });
  
        console.log("userprofile json: ", userProfileJson);
  
        this._userProfileService.saveUserProfile(userProfileJson).subscribe(
          (response) => {
            console.log('Профіль збережено на сервері: ', response);
            this.uploadPhoto(this.userProfile.userId, this.userInit.userName);
          },
          (error) => {
            console.error('Помилка збереження профілю:', error);
            console.error('Деталі помилки:', error.error.errors);
          }
        );
      },
      error: (err) => {
        console.error("error: ", err);
      }
    });
  }
  

  uploadPhoto(id: number, userName: string): void {
    const formData = new FormData();

    console.log("uploading Photo")
  
    formData.append('id', id.toString());
    formData.append('userName', userName);
    if (this.userProfile.photo) {
      const photoBlob = this.dataURItoBlob(this.userProfile.photo);
      formData.append('Photo', photoBlob, 'photo.jpg');
      console.log(photoBlob);
    }
    else {
      return;
    }

    this._userProfileService.uploadUserPhoto(formData).subscribe({
      next: (response) => {
        console.log('Фото завантажено:', response);
      },
      error: (err) => {
        console.log('Помилка завантаження фото:', err);
      }
    });
  }

  uploadPhotoIntoProfile(): void {  
    this._userProfileService.getUserProfilePhoto(this.userInit.userName).subscribe({
      next: (response: Blob) => {
        // Створюємо URL з отриманого Blob (файлу)
        const url = URL.createObjectURL(response);
  
        // Шукаємо елемент <img> та оновлюємо його src
        const imgElement = document.querySelector('.profile-photo') as HTMLImageElement;
        if (imgElement) {
          imgElement.src = url;
        }
      },
      error: (err) => {
        console.error('Не вдалося завантажити фотографію користувача:', err);
      }
    });
  }
  
  
  
  private dataURItoBlob(dataURI: string): Blob {
    const byteString = atob(dataURI.split(',')[1]);
    const mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0];
    const ab = new ArrayBuffer(byteString.length);
    const ia = new Uint8Array(ab);
    for (let i = 0; i < byteString.length; i++) {
      ia[i] = byteString.charCodeAt(i);
    }
    return new Blob([ab], { type: mimeString });
  }
  

  onPhotoUpload(event: Event): void {
    const input = event.target as HTMLInputElement;

    if (input.files && input.files[0]) {
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.userProfile.photo = e.target.result;
      };
      reader.readAsDataURL(input.files[0]);

      console.log('Фото завантажено');
    }
  }

  private async loadUserProfile(): Promise<void> {
    await this._userProfileService.getUserProfileData(this.userInit.userName).subscribe(
      {
        next: (response) =>
        {
          this.userProfile = response;
          if(this.userProfile.gender == "male")
            this.userProfile.gender = "Хлопець";
          else 
            this.userProfile.gender = "Дівчина";
          if(this.userProfile.interestedIn == "girls")
            this.userProfile.interestedIn = "Дівчата";
          else if(this.userProfile.interestedIn == "boys")
            this.userProfile.interestedIn = "Хлопці";
          else 
            this.userProfile.interestedIn = "Байдуже";
        },
        error: (err) =>
        {
          console.error("user profile erorr: ", err);
        }
      }
    );
  }
}
