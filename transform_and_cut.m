function [path_length, intensity_cut] = transform_and_cut(intensity,Z,transl_vec,input_fov_hori, output_fov_hori, input_fov_vert, output_fov_vert)
Z = Z';
intensity = intensity';
width_input = size(Z,1);
height_input = size(Z,2);

%Camera Parameters
cx_1 = width_input/2;
cy_1 = height_input/2;
fx_1 = cx_1 / tand(input_fov_hori/2);
fy_1 = cy_1 / tand(input_fov_vert/2);
C1 = [fx_1 0 cx_1;
      0 fy_1 cy_1;
      0 0 1];
  
fx_2 = fx_1;
fy_2 = fy_1;
cx_2 = fx_2 * tand(output_fov_hori/2);
cy_2 = fy_2 * tand(output_fov_vert/2);  
C2 = [fx_2 0 cx_2;
      0 fy_2 cy_2;
      0 0 1];
%%
X_min_SS = -tan(deg2rad(input_fov_hori/2));
X_max_SS = tan(deg2rad(input_fov_hori/2));
Y_min_SS = -tan(deg2rad(input_fov_vert/2));
Y_max_SS = tan(deg2rad(input_fov_vert/2));

intensity(Z==0) = NaN;
Z(Z==0) = NaN; % in this way X and Y are NaN for values of Z = 0
X = (Z*X_max_SS) .* (((repmat([1:width_input]',1,height_input)) - (width_input/2))/(width_input/2));
Y = (Z*Y_max_SS) .* (((repmat([1:height_input],width_input,1)) - (height_input/2))/(height_input/2));

world_coords(:,:,1) = X;
world_coords(:,:,2) = Y;
world_coords(:,:,3) = Z;
world_coords(:,:,4) = 1;

% radial_dist_uncut = sqrt(X.^2 + Y.^2 + Z.^2);
% radial_dist_uncut = radial_dist_uncut';



%%Transformation
P2 = eye(3);
P2(:,4) = [transl_vec'];
P2(:,4) = -P2(1:3,1:3)*P2(:,4);

T = C2*P2;
P = reshape(world_coords, width_input * height_input, 4) * T';
P_for_meters = reshape(world_coords, width_input * height_input, 4) * (P2)';
XYZ_transl_meters = reshape(P_for_meters, width_input, height_input, 3);
uv_s = reshape(P, width_input, height_input, 3);


uv_c2(:,:,1) = round(uv_s(:,:,1) ./ uv_s(:,:,3));
uv_c2(:,:,2) = round(uv_s(:,:,2) ./ uv_s(:,:,3));

uv_c2_x = uv_c2(:,:,1);
uv_c2_y = uv_c2(:,:,2);

uv_c2_x(isnan(uv_c2_x)) = 0;
uv_c2_y(isnan(uv_c2_y)) = 0;
uv_c2_x(uv_c2_x<0)=0;
uv_c2_y(uv_c2_y<0)=0;

[a,b] = find(uv_c2_x>0 & uv_c2_x<=2*cx_2);
x_start_uncut = min(a);
x_end_uncut = max(a);

[c,d] = find(uv_c2_y>0 & uv_c2_y<=2*cy_2);
y_start_uncut = min(d);
y_end_uncut = max(d)+1;


intensity_cut = NaN(2*round(cy_2), 2*round(cx_2));


cut_from_big_x = uv_c2_x(x_start_uncut:x_end_uncut, y_start_uncut:y_end_uncut);
cut_from_big_y = uv_c2_y(x_start_uncut:x_end_uncut, y_start_uncut:y_end_uncut);

cut_from_big_x(isnan(cut_from_big_x)) = 1;
cut_from_big_y(isnan(cut_from_big_y)) = 1;
cut_from_big_x(cut_from_big_x==0) = 1;
cut_from_big_y(cut_from_big_y==0) = 1;


linear_index = cut_from_big_y(:,:) + 2*round(cy_2) * (cut_from_big_x(:,:)-1);



intensity_cut(linear_index) = intensity(x_start_uncut:x_end_uncut,(y_start_uncut:y_end_uncut));
% radial_dist_cut = sqrt(mask_x.^2 + mask_y.^2 + mask_z.^2);
 

d1_x = XYZ_transl_meters(x_start_uncut:x_end_uncut,(y_start_uncut:y_end_uncut),1); 
d1_y = XYZ_transl_meters(x_start_uncut:x_end_uncut,(y_start_uncut:y_end_uncut),2); 
d1_z = XYZ_transl_meters(x_start_uncut:x_end_uncut,(y_start_uncut:y_end_uncut),3); 
d1 = sqrt(d1_x.^2 + d1_y.^2 + d1_z.^2);


d2_x = X(x_start_uncut:x_end_uncut, y_start_uncut:y_end_uncut); 
d2_y = Y(x_start_uncut:x_end_uncut, y_start_uncut:y_end_uncut); 
d2_z = Z(x_start_uncut:x_end_uncut, y_start_uncut:y_end_uncut); 
d2 = sqrt(d2_x.^2 + d2_y.^2 + d2_z.^2);


path_length = NaN(2*round(cy_2), 2*round(cx_2));
path_length(linear_index) = d1+d2;
% path_length = path_length';
% intensity_cut = intensity_cut';


% azimuth = atan(mask_x./mask_z);





