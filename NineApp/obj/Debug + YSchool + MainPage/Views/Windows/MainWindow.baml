   M S B A M L   `   `   `  ���� T  PMahApps.Metro, Version=1.1.2.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  B >NineApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:Nine.Views.ControlsNine.Views.Controls #   Nine.Views.Windows.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     OMahApps;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     6Controls!clr-namespace:Nine.Views.Controls       � Icon$#  /Resources/NineRedFavicon.ico�� � Title$- 'NINE - New Interactions, New Educations��(   "MahApps.Metro.Controls.MetroWindow   EnableDWMDropShadow .    WindowTransitionsEnabled .   � WindowStartupLocation$ CenterScreen=� � WindowState$ Normal=�$	��450��$	��600��   IgnoreTaskbarOnMaximize . $	��700��$
��1300��   ShowTitleBar �� *WindowState+ �� RelativeSource �� *Self+   WindowStateToBoolean	 �� 	Converter#	 [  
 �� ConverterParameter$
 Normal��   IconTemplate �� .R�� #��z��#��z��
������  �� EdgeMode$ Aliased=� �� BitmapScalingMode$ HighQuality=�.+��� $+��%/Resources/NineWhiteMarginFavicon.ico��   Flyouts +   %MahApps.Metro.Controls.FlyoutsControl  .@�#   MahApps.Metro.Controls.Flyout  -     Name$ GeneralFlyoutMenu��   Header$  ��   Position$
 Left=�$	��180��   IsPinned .  .���  � RowDefinitions �� $
��Auto ��� $��* ��� $
��Auto �.+��� -    ��Name$ FlyoutContainer��$��0��$��Stretch=� � 	Resources  �� 
TargetType �� Property �� Value��%4   '��       �� " ��.���  � ��$ 	10,10,0,0q�.+�+  %Nine.Views.Controls.UserFlyoutControl  �� 
�������$
��15 5q��� $��Ouvrir��  FlyoutButtonStyle#���  ���� *
OpenFile+
 � Tag !�   appbar_folder !� Visual# [  j� Stretch$
 Fill=��� $��Enregistrer��#���  ���� *
SaveFile+ !�   appbar_save# [ $
 Fill=��� $��Partager leçon��#���  ���� *ShareLesson+ !�   appbar_share# [ $
 Fill=��� $��Fermer��#���  ���� *	CloseFile+ !�   appbar_undo# [ $
 Fill=��� $��Quitter��#���  ���� *MainWindowClose+ !�   appbar_close# [ $
 Fill=��� $��2��$��À propos de Nine��#���  ���� *
AboutClick+ !�   appbar_quote_right# [ $
 Fill=�  -   $ LessonFlyoutMenu��$ Bottom=� .  .���� $��Stretch=� �� Orientation$ 
Horizontal=�.+��  � Orientation$ Vertical=�$��6 0 0 6q�.+��� $��Stretch=� �� TextAlignment$ Center=����#FFF��   MetroCircleButtonStyle#��[ $��50��$��50��$��0q����� *RenameLesson+.���� $��26��$��26�����#FFF J� OpacityMask !� $ Uniform=� 	 appbar_interface_textbox# � 	 �� $��Stretch=�$ Center=�
Renommer� $ Vertical=�$��6 0 0 6q�.+��� $��Stretch=�$ Center=����#FFF�� #��[ $��50��$��50��$��0q����� *ShareLesson+.���� $��22��$��22�����#FFF !� $ Uniform=�# �  �� $��Stretch=�$ Center=�
Partager� $ Vertical=�$��6 0 0 6q�.+��� $��Stretch=�$ Center=����#FFF�� #��[ $��50��$��50��$��0q����� *DeleteLesson+.���� $��22��$��22�����#FFF !� $ Uniform=� 
 appbar_delete# � 
 �� $��Stretch=�$ Center=�	Supprimer%  Nine.Views.Controls.ShareFlyout      IsModal  . 