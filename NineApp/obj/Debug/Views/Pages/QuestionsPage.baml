   M S B A M L   `   `   `  ����B  >NineApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:Nine.Views.ControlsNine.Views.Controls  5      E ARadialMenu, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM5clr-namespace:RadialMenu.Controls;assembly=RadialMenuRadialMenu.Controls 5      T PMahApps.Metro, Version=1.1.2.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls 5      'clr-namespace:Nine.Tools
Nine.Tools  5      $    Nine.Views.Pages.QuestionsPage   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      0Controls!clr-namespace:Nine.Views.Controls  5      F
RadialMenu5clr-namespace:RadialMenu.Controls;assembly=RadialMenu  5      IMahApps;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro  5      *BetterPopupclr-namespace:Nine.Tools  5      -   5          � 	Resources  5         BottomSideBar �� 
TargetType �� Triggers �� Binding �� ElementName �� Value N� EnterActions �� TargetProperty O� KeyTime	 �� 	KeySpline
 O� Value N� ExitActions  LeftSideBar��%  &	        &	 �    �� 5   
   " �6&    5      �� 5       6   �� *DataContext.IsFullscreenMode+$ 
LayerStack�� True6g    5      �� 5      .��5      �� .��5   "   U� $ Height�6@   .\�5   &   �� $ 0:0:0.75��6;   $	 0.25,1,0.05,1��6N   
 06h   5   #   5      5       5      �� 5      .��5       �� .��5!   "   U� $ Height�6@   .\�5"   &   �� $ 0:0:0.75��6;   $	 0.25,1,0.05,1��6N   
 606h   5#   #   5$      5%      5(      5*      �� 5-   
   " �6$    5.      �� 5/       6   �� *DataContext.IsFullscreenMode+$ 
LayerStack�� True6g    52      �� 53      .��54      �� .��55   "   U� $ Width�6@   .\�56   &   �� $ 0:0:0.75��6;   $	 0.25,1,0.05,1��6N   
 06h   57   #   58      59       5=      �� 5>      .��5?      �� .��5@   "   U� $ Width�6@   .\�5A   &   �� $ 0:0:0.75��6;   $	 0.25,1,0.05,1��6N   
 2006h   5B   #   5C      5D      5G      5I      5J      .��5L      ]�   SideBarBackgroundBrush#��[ 6   .+�5N   
      Nine.Tools.BetterPopup  -   5          Name$ popup��5N   "   $��Center=�6T    � AllowsTransparency . $
��Fade=�5O   <    � DataContext 5N   1   �� *
LessonPage+��5O   R   �� *PopupIsOpen+.��5P      � ���	#FFE8E8E8.+�5Q      �� $��Création d'exercices��6   $��Center=�69   $��Center=�6V   .��5R      �� .+�5S      �� $	��Nom��6    �� 5T      -   5         ��Name$ nameTB��5T   "   $	��3 0q�6P   ��62   �� *ExerciseName+6P   � 5U      $��0,10,0,5q�6    � ColumnDefinitions 5V      �� 5W   "   �� 5X   "   �� 5Y   "   .+�5[      ��   TileButtonStyle#��[ 6%   $��0��6N   $��95��6^   $��95��6j   $	��3 3q�5\   %   $	��QCM��6]   ��62   �� *BeginToComposeExercise+.��5]   "   �  � RowDefinitions 5^   &   �� 5_   *   �� 5`   *   $��30 �68   .+�5b   &   �� 
�������$��35��6=   $��35��6I   $��0��6T    J� OpacityMask 5c   *   !� 5d   .     checkbox_icon !� Visual# [ 6:    j� Stretch$
 Fill=�6b   5f   '   �� 5g   &   $��1��60   
�������$��18(�6P   $��Segoe UI)�6^   $��Center=�5h   0   $��Center=�6M   $��3,3,3,6q�6h   QCM5j   '   5k   #   5l      �� 5m      #��[ 6%   $��1��6N   $��95��6^   $��95��6j   $	��3 3q�5n   %   $	��QCU��6]   ��62   �� *BeginToComposeExercise+.��5o   "   �  5p   &   �� 5q   *   �� 5r   *   $��30 �68   .+�5t   &   �� 
�������$��35��6=   $��35��6I   $��0��6T    5u   *   !� 5v   .     
radio_icon# [ 6:   $
 Fill=�6_   5x   '   �� 5y   &   $��1��60   
�������$��18(�6P   $��Segoe UI)�6^   $��Center=�5z   0   $��Center=�6M   $��3,3,3,6q�6h   QCU5|   '   5}   #   5      �� 5�      #��[ 6%   $��2��6N   $��95��6^   $��95��6j   $	��3 3q�5�   %   $��	Graphical��5�   %   ��5�   2   �� *BeginToComposeExercise+.��5�   "   �  5�   &   �� 5�   *   �� 5�   *   $��30 �68   .+�5�   &   �� 
�������$��35��6=   $��35��6I   $��0��6T    5�   *   !� 5�   .     graphic_icon# [ 6:   $
 Fill=�6a   5�   '   �� 5�   &   $��1��60   
�������$��18(�6P   $��Segoe UI)�6^   $��Center=�5�   0   $��Center=�6M   $��3,3,3,6q�6h   	Graphique5�   '   5�   #   5�      5�      �� 5�      #��[ 6!   $��22��6J   
�������
�������$��3,0,3,3q�5�   !   ��5�   !   �� *Width+$ popup����5�   2   �� *
ClosePopup+.��5�      �� $��Annuler��6(   $��Center=�67   $��Center=�6T   5�      5�      5�      5�      5�      � 5�   
   $��Bottom=�5�      .+�5�          Nine.Views.Controls.AppBar  -   5          Name$ AppBar��5�      $��0��5�       5�      �� *DataContext.AppBarState+$ 
LayerStack��
 � Tag 5�      �� *DataContext+$ 
LayerStack��5�      � 5�   
   $��Bottom=�5�      $��60��5�      #��[  5�      .+�5�      '   !Nine.Views.Controls.BottomSideBar  -   5          Name$ BottomSideBar��5�       5�      �� *DataContext+$ 
LayerStack��5�      ]� 5�   
   $	��Top=�6   .  (System.Windows.Interactivity.Interaction   Triggers 5�      /  )System.Windows.Interactivity.EventTrigger  5�         	EventName$ PreviewTouchDown��6!   .  (System.Windows.Interactivity.TriggerBase   Actions. 5�      6  0System.Windows.Interactivity.InvokeCommandAction     Command 6,   �� *DataContext.Close+$ AppBar��5�        5�      $ PreviewMouseDown��6!   . 5�         6,   �� *DataContext.Close+$ AppBar��5�      .+�5�      � $
��Left=�5�      $	��200��5�      #��[ 5�      .+�5�      )   #Nine.Views.Controls.QuestionSideBar  -   5          Name$ LessonSidebar��5�       5�      �� *
LessonPage+5�      � 5�      -   5        $��Right=�5�        ContentBackgroundBrush#��[ 5�      .+�5�      ��  K� ZIndex$ 10��6   }�. .+�5�      %	   Nine.Views.Controls.RadialMenus	  -   5         	 Name$ 
RadialMenu��5�      $ 10��5�       5�      �� *DataContext+$ 
LayerStack�� �� Left 5�      �� *RadialMenuState.LeftPosition+
  �� Top  5�      �� *RadialMenuState.TopPosition+/
   )Nine.Views.Controls.RadialMenuStateButton
  5�      -	   5        ! 
 Name$! RadialMenuOpenButton��5�      $ 0��5�      $	  140��5�      " �� Right$	" 155��5�       5�      �� $ LessonContainer�� 5�      �� *DataContext+$ 
LayerStack��5�      5�      +   %Nine.Views.Controls.TouchScrollViewer  5�      -
   5        #  Name$# LessonContainer��5�      $ 0��5�      #$   IsContentManipulationEnabled$ . %   HorizontalOffset% 5�      �� *'%DataContext.Viewport.HorizontalOffset+$ 
LayerStack��& �� Mode$& TwoWay=�'   VerticalOffset' 5�      �� *%#DataContext.Viewport.VerticalOffset+$ 
LayerStack��$& TwoWay=�.��5�      � -   5        ( �Name$( LessonContent��5�      ) �� IsFlicksEnabled) .  * �� IsPressAndHoldEnabled* .  + �� IsTapFeedbackEnabled+ .  , �� IsTouchFeedbackEnabled, .   5�        5�      $ PreviewTouchDown��6-   . 5�   "      5�   %   �� *DataContext.CloseRadialMenu+$ 
LayerStack��5�        5�      $ PreviewMouseDown��6-   . 5�   "      5�   %   �� *DataContext.CloseRadialMenu+$ 
LayerStack��5      - � LayoutTransform- 5     �� 5     . �� ScaleX. 5  !   �� *DataContext.Viewport.Scale+$ 
LayerStack��/ �� ScaleY/ 5  !   �� *DataContext.Viewport.Scale+$ 
LayerStack��.+�5     /   )Nine.Views.Controls.DrawingAreaBackground  -   5        0  Name$0 DrawingAreaBound��5     $ 0��5     $
��2800��5     $
��2000��5     $   Nine.Views.Controls.LayerStack  5     -   5        1  Name$1 
LayerStack��5     $ 10��5      5     �� *LayerStackDC+��5     �� *Width+$ DrawingAreaBound����5     �� *Height+$ DrawingAreaBound��5     5     5     5     5      5"     