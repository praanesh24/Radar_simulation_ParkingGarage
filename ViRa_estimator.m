function [result] = ViRa_estimator(intensity,distance, distance_previous,delta_t, c0, samplesNumber, chirpsNumber,fs, LowerChirpFrequency, ChirpRate )
    
    distancefin = distance;
    velocity = ((distancefin - distance_previous)) ./ delta_t;
% velocity = 0;
    
    result = zeros(chirpsNumber,samplesNumber);

    
%     % Make them 3rd in order to not use for loop on samples
%     distancefin = repmat(distancefin,1,1,samplesNumber);
%     distance = repmat(distance,1,1,samplesNumber);
%     velocity = repmat(velocity,1,1,samplesNumber);
    
    
    for chirps = 1 : chirpsNumber %chirp loop
%         tic
% %         tau = (2/c0) * (distancefin + (velocity * (samplesNumber / fs)*chirps)); % time delay 
% %         f1 = 2*pi*LowerChirpFrequency .* tau;
% %         f3 = -pi * ChirpRate.*tau.*tau;
% % 
% %         
% %         tic
% %         
% %         parfor samples = 1: samplesNumber
% %         
% %             t = (samples) / fs;
% % 
% %             f2 = 2 * pi*ChirpRate*tau*t;
% %             fn = (f1 + f2 + f3);
% % 
% %             solidrx = ( intensity .* cos(fn ));
% %             solidrx(isnan(solidrx))=0;
% % 
% %             result(chirps, samples) = sum(solidrx,'all');
% %         
% %         end
% %         toc
       
              
%         t = [(1:samplesNumber)./fs];
% %         tau_interm_1 = kron(t,(2/c0)*distancefin);
% %         tau_interm_1 = reshape(tau_interm_1,[size(distance,1), size(distance,2),samplesNumber]);
% %         tau_interm_2 = kron(t,(2*chirps/c0)*velocity);
% %         tau_interm_2 = reshape(tau_interm_2,[size(distance,1), size(distance,2),samplesNumber]);
% %         tau = tau_interm_1 + tau_interm_2;
% 
%         tau_interm_1 = 
%         
%         f1 = 2*pi*LowerChirpFrequency .* tau;
%         f2 = (2 * pi*ChirpRate.*tau);
%         f3 = -pi * ChirpRate.*tau.*tau;
%         
%         fn = (f1 + f2 + f3);
%         solidrx = repmat(intensity, 1,1, samplesNumber).*cos(fn);
%         solid_sum = squeeze(sum(solidrx,[1,2],'omitnan'));
%         result(chirps,:) = solid_sum;
%         
%     

        
        
        
        
        
        
        
        
        
        t = [(1:samplesNumber)./fs];
        tau = (2/c0) * (distancefin + (velocity * (samplesNumber / fs)*chirps)); % time delay 
        
        
        
%         tau = (2/c0) * (distancefin + (velocity * (samplesNumber / fs)*chirps)); % time delay 
        f1 = 2*pi*LowerChirpFrequency .* tau;
        f3 = -pi * ChirpRate.*tau.*tau;

        f2 = (2 * pi*ChirpRate.*tau);
        f2 = kron(t,f2);
        f2 = reshape(f2,[size(tau,1), size(tau,2),samplesNumber]);
        
        fn = (f1 + f2 + f3);
%         solidrx = repmat(intensity.*(1./(distancefin.^2)), 1,1, samplesNumber).*cos(fn);
        solidrx = repmat(intensity, 1,1, samplesNumber).*cos(fn);
        solid_sum = squeeze(sum(solidrx,[1,2],'omitnan'));
        result(chirps,:) = solid_sum;
        

        %  distancefin = distance + (velocity * chirps * (samples / fs));
        distancefin = distance + (velocity * chirps * (samplesNumber / fs));
          
%         toc
    end
    
    
    
  



    
%         for samples = 1: samplesNumber % samples loop
% %             tic
% %             t = (samples) / fs;
% % 
% % 
% %             f2 = 2 * pi*ChirpRate*tau*t;
% % 
% % 
% %             fn = (f1 + f2 + f3);
% % 
% %          
% %              solidrx = ( intensity .* cos(fn ));
% %             solidrx(isnan(solidrx))=0;
% %             
%             
%             
%   computing_solidrx_time = toc
%   tic
%             result(chirps, samples) = sum(solidrx,'all');
% sum_time = toc
%         end

        
%         distancefin = distance + (velocity * chirps * (samples / fs));
% toc
        
%         delta_dist = kron(t,velocity.*chirps);
%         delta_dist = reshape(delta_dist,[size(tau,1), size(tau,2),samplesNumber]);
%         distancefin = distance + delta_dist;



