   M S B A M L   `   `   `  ����B  >NineApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:Nine.Views.ControlsNine.Views.Controls  5      E ARadialMenu, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM5clr-namespace:RadialMenu.Controls;assembly=RadialMenuRadialMenu.Controls 5      T PMahApps.Metro, Version=1.1.2.0, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls 5      $    Nine.Views.Pages.FreeNotesPage   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      0Controls!clr-namespace:Nine.Views.Controls  5      F
RadialMenu5clr-namespace:RadialMenu.Controls;assembly=RadialMenu  5      IMahApps;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro  5      -   5          � 	Resources  5         BottomSideBar �� 
TargetType �� Triggers �� Binding �� ElementName �� Value N� EnterActions �� TargetProperty O� KeyTime	 �� 	KeySpline
 O� Value N� ExitActions  LeftSideBar��%  &	        &	 �    �� 5   
   " �6&    5      �� 5       6   �� *DataContext.IsFullscreenMode+$ 
LayerStack�� True6g    5      �� 5      .��5      �� .��5   "   U� $ Height�6@   .\�5   &   �� $ 0:0:0.75��6;   $	 0.25,1,0.05,1��6N   
 06h   5   #   5      5       5      �� 5      .��5      �� .��5    "   U� $ Height�6@   .\�5!   &   �� $ 0:0:0.75��6;   $	 0.25,1,0.05,1��6N   
 606h   5"   #   5#      5$      5'      5)      �� 5,   
   " �6$    5-      �� 5.       6   �� *DataContext.IsFullscreenMode+$ 
LayerStack�� True6g    51      �� 52      .��53      �� .��54   "   U� $ Width�6@   .\�55   &   �� $ 0:0:0.75��6;   $	 0.25,1,0.05,1��6N   
 06h   56   #   57      58       5<      �� 5=      .��5>      �� .��5?   "   U� $ Width�6@   .\�5@   &   �� $ 0:0:0.75��6;   $	 0.25,1,0.05,1��6N   
 2006h   5A   #   5B      5C      5F      5H      5I      .��5K      ]�   SideBarBackgroundBrush#��[ 6   .+�5N   
   � $��Bottom=�5O      .+�5P          Nine.Views.Controls.AppBar  -   5          Name$ AppBar��5Q      $��0��5R       � DataContext 5S      �� *DataContext.AppBarState+$ 
LayerStack��
 � Tag 5T      �� *DataContext+$ 
LayerStack��5U      � 5X   
   $��Bottom=�5Y      $��60��5Z      #��[  5[      .+�5\      '   !Nine.Views.Controls.BottomSideBar  -   5          Name$ BottomSideBar��5]       5^      �� *DataContext+$ 
LayerStack��5_      ]� 5c   
   $	��Top=�6   .  (System.Windows.Interactivity.Interaction   Triggers 5e      /  )System.Windows.Interactivity.EventTrigger  5f         	EventName$ PreviewTouchDown��6!   .  (System.Windows.Interactivity.TriggerBase   Actions. 5g      6  0System.Windows.Interactivity.InvokeCommandAction     Command 6,   �� *DataContext.Close+$ AppBar��5h        5i      $ PreviewMouseDown��6!   . 5j         6,   �� *DataContext.Close+$ AppBar��5k      .+�5o      � $
��Left=�5p      $	��200��5q      #��[ 5r      .+�5s      !   Nine.Views.Controls.SideBar  -   5          Name$ LessonSidebar��5t       5u      �� *
LessonPage+5v      � 5y      -   5        $��Right=�5z        ContentBackgroundBrush#��[ 5}      .+�5      ��  K� ZIndex$ 10��6   }�. .+�5�      %   Nine.Views.Controls.RadialMenus  -   5          Name$ 
RadialMenu��5�      $ 10��5�       5�      �� *DataContext+$ 
LayerStack�� �� Left 5�      �� *RadialMenuState.LeftPosition+
 �� Top 5�      �� *RadialMenuState.TopPosition+/	   )Nine.Views.Controls.RadialMenuStateButton	  5�      -   5         	 Name$ RadialMenuOpenButton��5�      $ 0��5�      $	 140��5�       �� Right$	 155��5�       5�      �� $ LessonContainer�� 5�      �� *DataContext+$ 
LayerStack��5�      5�      +
   %Nine.Views.Controls.TouchScrollViewer
  5�      -   5         
 Name$ LessonContainer��5�      $ 0��5�      # 
  IsContentManipulationEnabled .  
  HorizontalOffset 5�      �� *'%DataContext.Viewport.HorizontalOffset+$ 
LayerStack�� �� Mode$ TwoWay=� 
  VerticalOffset 5�      �� *%#DataContext.Viewport.VerticalOffset+$ 
LayerStack��$ TwoWay=�.��5�      � -	   5          �Name$  LessonContent��5�      ! �� IsFlicksEnabled! .  " �� IsPressAndHoldEnabled" .  # �� IsTapFeedbackEnabled# .  $ �� IsTouchFeedbackEnabled$ .   5�        5�      $ PreviewTouchDown��6-   . 5�   "      5�   %   �� *DataContext.CloseRadialMenu+$ 
LayerStack��5�        5�      $ PreviewMouseDown��6-   . 5�   "      5�   %   �� *DataContext.CloseRadialMenu+$ 
LayerStack��5�      % � LayoutTransform% 5�      �� 5�      & �� ScaleX& 5�   !   �� *DataContext.Viewport.Scale+$ 
LayerStack��' �� ScaleY' 5�   !   �� *DataContext.Viewport.Scale+$ 
LayerStack��.+�5�      /   )Nine.Views.Controls.DrawingAreaBackground  -
   5        (  Name$( DrawingAreaBound��5�      $ 0��5�      $
��2800��5�      $
��2000��5�      $   Nine.Views.Controls.LayerStack  5�      -   5        )  Name$) 
LayerStack��5�      $ 10��5�       5�      �� *LayerStackDC+��5�      �� *Width+$ DrawingAreaBound����5�      �� *Height+$ DrawingAreaBound��5�      5�      5�      5�      5�      5�      